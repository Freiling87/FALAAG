#	FREE AGENT: Life as a Goon<sup>(Working Title)</sup>
**Game Design Document**

|Date|Version|Author|Notes|
|:---:|---:|:---|:---|
|2022-03-13|v0.1|Ted Bunny|- General concepts

#	Game Overview

##		Concept
This project can be up to three things in one, depending on how the player chooses to use it:
  - A procedurally generated interactive crime thriller novel that can simply be played like any other game.
  - An engine with modular downloadable content packs that can edit or augment the setting, game mechanics, or writing style. As above, but much more replayable.
  - Content creation & sharing tools for the above. The player can create playable content with a minimum of technical aptitude required.

##		Medium
All game information is presented in prose. This is done by permutating and combining words, phrases and clauses. The resulting text is not expected to be as novel as a book by a real author, but will hopefully provide enough of a reasonable facsimile to create a unique and compelling experience, and ideally to inspire players to create custom content.

The complete eschewal of graphics and audio has the following benefits:
  - More processing power can be allocated to deep modeling
  - The code is simpler
  - Custom content is easier to create

##		Genre
This project does not fall into an existing genre. It has similarities with the following, with major caveats:
  - **Roguelike:** Turn-based, procedurally generated, permadeath, with emergent gameplay mechanics. But Roguelikes are too focused on combat and acquisition, whereas I want to emphasize storytelling and evocative details.
  - **Text Adventure:** Text-based & atmospheric in order to recruit the reader's imagination. But text-adventures are often hand-crafted and limited to certain pre-defined options. Instead, we will prefer emergence over content by fiat, where appropriate.

##		Target Audience
This is hard to estimate. Text-only games are a niche genre, and no major releases exist for comparison. Still, I think more than a few people enjoy books as well as games, and would appreciate a merger of the two. *Terra incognita* can be a wasteland or an Eden!

##		Gameplay
The player can freely explore procedurally-generated maps, and interact with their contents as they choose. They are not limited to prescribed actions as in the classic text adventures. Gameplay mechanics are flexible enough that the player can portray e.g. a cat burglar, a manipulative con man, a corrupt or upright cop, a private bodyguard, etc.

The world outside of player choice and actions should feel similarly alive. Non-player characters will act intelligently. The narrator will point out sounds and smells in evocative fashion. The player character might have an occasional internal monologue, if they're an introvert.

Dialogue is a complex dance of hidden motives and power-talk. But this is a much later goal that is currently out of scope.

##		Plot
Plot is emergent. As in RimWorld or Streets of Rogue, the plot will emerge from the facts the game presents. The engine will identify particular scenarios that imply a plot, and support it with its exposition accordingly. This might be assisted by the player explicitly declaring the intent of their character in a given scene.

##		Official Content
There is no canonical setting or writing style for ToME. It will ship with one or two content packs as robust examples, but the player is not limited to those. 

The included setting, FALAAG, roughly approximates the "mean streets" depicted in early Scorcese films. It may have some futuristic twists that might account for any lapses in simulation - e.g., the police are not very present because they've been mostly privatized.

The included Narrator & Dialogue resemble a pulp novel. Exposition, speech and plot are simple but not mechanical.

##		Custom Content
Player-made content is a high priority and metric of the project's success. Content packs are no more complicated than editing XAML files with a simple string markup syntax. The engine should ship with Content Creation Tools to further facilitate this.

All of the content modules below would be editable by players:
  - Narrator style
  - Dialogue style
  - Prefab map "chunks" for procedurally-generated placement
  - All physical objects: Walls, materials, items, furniture
  - Game Attributes, skills, and the formulae that go into them

##		Design Principles
I take issue with several trends in game design, and this project is partly a demonstration to their contrary.

###			Spaces > Places
Spatial matters tend to dominate most games. In games with grid-based maps, the cells in that grid indicate exact dimensions that the player can use predictably. As a result, visual representation is paramount. 

The map cell in this game is more like that in a text adventure - a psychic* space rather than a geometric one. There is no standard height, width, or depth. A Map Cell could be a dining room or a closet, cavern or crevice. A cell can be occupied by several people, and it could be cluttered or empty. By eschewing the granular spatiality of most games, we can focus on the things and concepts that matter, and create them by fiat. 

Movement within the Cell is simulated through status effects, such as "Hidden Behind Cover," "Blocking Door to West," "Locked in Bathroom Stall," etc. This distills movement and action to only those things which matter, eliminating the trivialities of room measurements and counting steps.

The obvious downside to this is that interior floorplans will be "fuzzy." Cutting through a hallway's wall, for example, might yield an equal chance to come out through a pantry as through a kitchen, contrary to the same scenario in real life. There will be special considerations to make any spatially-crucial considerations predictable for the player.

* Psychic as in "from the psyche," not magical mind powers.

###			High-Concept > High-Tech
By avoiding graphics and audio, the project can focus on algorithms and content. If you're a reader, you know that the Theater of the Mind is a very powerful substitute. Games like Dwarf Fortress and RimWorld are proof of this.

The code itself is simple, and as of now relies only on the following technologies: C#, WPF, XAML, JSON. For more complex algorithms, I may import libraries such as A-Star for pathfinding. 

###			Feels > Reals
Most games embrace "crunchy mechanics": they directly report the player character's physical fatigue as "22/100," or say things like "Critical Hit" to describe a marginally better attack among a monotone of traded blows. These are assumed to be adequate when the game is merely a vehicle for gameplay. Instead, we will translate mechanics into natural language with emotional valence. For example, suppose the player's character hits a street thug with a bat. 

A traditional roguelike might report: 
  `"You swing your bat at the thug. Critical Hit! You deal 12 damage."`

The desired report for this project is something more like:
  `"The bat nearly slips out of your sweaty grip as you swing it, but it connects. You hear a wet crunch as it hits him in the ribs, and he staggers in pain."`

This is more complex, but it's the main point of this project. To be clear: gameplay is not being simplified - merely obscured by a layer that looks like a novel.

###			Avoiding Triviality
In film & TV, every second of footage matters. Lines and scenes that have no significance don’t make it into the script. The same goes, to a lesser degree, for writing.

In contrast, games often embrace the trivial because their gameplay is inherently rewarding. These forms of "filler content" can be highly engaging, but repetitive and story-irrelevant:
  - Countless trivial yet ridiculously elaborate combat encounters, many of which pose zero actual threat to the player
  - Crafting systems so time-consuming that they can feel like a part-time job
  - Robotic dialogue that conveys only gameplay-relevant information or, at best, puddle-deep worldbuilding
  - Rapid increase in the player character's powers ("Power Spiral"), relegating anything with relevance to human sensibilities far into the background.

My intended solution for this is only to present what matters: Combat should be fast, high-stakes, and plot-pivotal as it would be in a film; Crafting should be no more than the setup for things that actually matter to the plot; Dialogue should be tense, meaningful and layered with motive; and the characters should remain relateable since they are central.


#	Theater of Mind Engine (ToME)
ToME is the pearl of this project. Any content packaged with it is simply a tech demo.

It consists of multiple layers:
  - Game: The actual RPG-like objects that interact to comprise gameplay.
  - Writer: The system which translates Gameplay into Prose.
  - GUI: The player's controls.

##		Game

###			World

###			Characters

###			Actions

##		Writer

###			Wording
This refers to the sentences, clauses, fragments and words that constitute the game's prose output. They are conjugated, ordered, and concatenated according to many variables passed through simple algorithms.

##			Dynamics & Scenes
A Dynamic is a set of criteria for an atom of emotional valence. It can affect the tone of dialogue or narration, as well as the gameplay itself.

Some examples:
  - The player makes a lockpicking attempt. Behind the scenes, it's because he rolled poorly for his Fine Motor Skill attribute. This is partly due to a bad dice roll, but party because he has the "Exhausted" status effect, which reduces Fine Motor Skill. The Writer attaches a subordinate clause to the attempt's report, "Your hands still shaking from exhaustion," and the result follows as the main clause. Additional algorithms prevent the same phrase from occurring repetitively.
  - The player is badly losing a fight, and it's not because of bad luck. The writer starts using Wording that is tagged with the "Hopeless" dynamic.
  - The player is sneaking around picking locks, and no one is aware of their presence. ToME identifies the "Stealth" dynamic, and makes itself more likely to report audible sounds.
  - The player and an enemy are shooting at each other. The game deactivates the "Polite Conversation" dynamic, now less likely to report on facial expressions. 
  - An enemy means to capture the player, and the player runs. ToME identifies a "Chase" dynamic, and this is used to declare a "Foot Chase" scene. The game starts reporting more in regards to long-range vision and foreseeable physical obstacles to the chase.

###			Content Markup Syntax


#	Release

##		Platforms
- Windows

This is a low-tech project, so porting may be trivial.

##		Business Model
This is primarily a learning project and creative outlet, but I am open to release if it nears completion. The market for text adventures in the modern day is at best unknown, and at worst nonexistent.

Release would include the Engine and one or two examples of content packs. Future official content packs could be released as DLCs, but player-made content should be simple enough that they'd not be necessary for enjoyment of the game.