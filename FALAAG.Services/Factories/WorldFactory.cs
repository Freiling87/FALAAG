using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FALAAG.Models;
using FALAAG.Models.Shared;

namespace FALAAG.Factories
{
    public static class WorldFactory
	{
        private const string dataFilepath = ".\\GameData\\Cells.xml";

        public static World CreateWorld()
        {
            World world = new World();

            if (File.Exists(dataFilepath))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(dataFilepath));

                string rootImagePath =
                    data.SelectSingleNode("/Cells")
                        .AttributeAsString("RootImagePath");

                LoadLocationsFromNodes(world,
                                       rootImagePath,
                                       data.SelectNodes("/Cells/Cell"));
            }
            else
                throw new FileNotFoundException($"Missing data file: {dataFilepath}");

            return world;
        }

        private static void LoadLocationsFromNodes(World world, string rootImagePath, XmlNodeList nodes)
        {
            if (nodes == null)
                return;

            foreach (XmlNode node in nodes)
            {
                Cell cell = new Cell(
                    node.AttributeAsInt("X"),
                    node.AttributeAsInt("Y"),
                    node.AttributeAsInt("Z"),
                    node.AttributeAsString("Name"),
                    node.AttributeAsString("Description"),
                    $".{rootImagePath}{node.AttributeAsString("ImageFilename")}");

                AddAutomats(cell, node.SelectNodes("./Automats/Automat"));
                AddWalls(cell, node.SelectNodes("./Walls/Wall"));
                AddNPCs(cell, node.SelectNodes("./NPCs/NPC"));
                AddJobs(cell, node.SelectNodes("./Jobs/Job"));

                world.AddCell(cell);
            }
        }

        #region Cell Population
        private static void AddNPCs(Cell cell, XmlNodeList npcs)
        {
            if (npcs == null)
                return;

            foreach (XmlNode node in npcs)
                cell.AddNPC(
                    node.AttributeAsString("ID"),
                    node.AttributeAsInt("Percent"));
        }

        private static void AddWalls(Cell cell, XmlNodeList walls)
		{
            if (walls.Count == 0)
                return;

            foreach (XmlNode node in walls)
			{
                Wall wall = WallFactory.GetWallByID(node.AttributeAsString("ID"));
                cell.AddWall(wall, Enum.Parse<Direction>(node.AttributeAsString("Direction")));
			}
		}

        private static void AddJobs(Cell cell, XmlNodeList jobs)
        {
            if (jobs == null)
                return;

            foreach (XmlNode questNode in jobs)
                cell.JobsHere.Add(JobFactory.GetJobByID(questNode.AttributeAsString("ID")));
        }

        private static void AddAutomat(Cell cell, XmlNode automatHere)
        {
            // TODO: Move to Cell.
            if (automatHere == null)
                return;

            cell.AutomatHere =
                AutomatFactory.GetAutomatByID(automatHere.AttributeAsString("ID"));
        }

        private static void AddAutomats(Cell cell, XmlNodeList automatsHere)
        {
            if (automatsHere == null)
                return;

            foreach (XmlNode automat in automatsHere)
                cell.Automats.Add(AutomatFactory.GetAutomatByID(automat.AttributeAsString("ID")));
        }

        #endregion
        #region Cell Operations
        private static bool CanCellFit(Cell cell, int x, int y, int z, bool flipX = false, bool flipY = false, bool flipZ = false, bool rotate = false)
        {


            return true;
        }

        #endregion
        #region Chunk Operations
        private static void FlipChunk(Chunk chunk, CartesianAxis axis)
		{

        }

        private static void PlaceChunk(Chunk chunkTemplate, int mapX, int mapY, int mapZ, bool placeByOrigin = true, bool flipX = false, bool flipY = false, bool flipZ = false, bool rotate = false)
		{
            Chunk chunk = chunkTemplate.Clone();

            if (flipX)
                FlipChunk(chunk, CartesianAxis.X);

            if (flipY)
                FlipChunk(chunk, CartesianAxis.Y);

            if (flipZ)
                FlipChunk(chunk, CartesianAxis.Z);

            if (rotate)
                RotateChunk(chunk);


            // Since Chunk transformations involve cloning, there is the possibility of redundancy here since we're cloning cells as well. Whatever the final form, ensure that we're not making a bunch of unused objects that never get deleted.
            foreach (Cell cellTemplate in chunk.Cells)
			{
                Cell cell = cellTemplate.Clone();

                cell.X += mapX;
                cell.Y += mapY;
                cell.Z += mapZ;
			}

		}

        private static void RotateChunk(Chunk chunk, int rotations = 1, bool Clockwise = true)
        {

        }

        private static bool CanChunkFit(Chunk chunk, int mapX, int mapY, int mapZ, bool placeByOrigin = true, bool flipX = false, bool flipY = false, bool flipZ = false, bool rotate = false)
        {


            return true;
        }
		#endregion
    }
}
