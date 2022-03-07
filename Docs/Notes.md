# FREE AGENT: Life as a Goon
Concept: A Text-based RPG inspired by crime-thriller or crime-action films.
##		Issue Tracking
###			Quest took 1 instead of 12 to complete, but checked for 12 correctly
###			Job not being added to NPC as of lesson 14.5
##	SOSCSRPG Tutorial
- I am struggling to understand the pattern of creating an extra class as a "joiner," in the case of ItemQuantity, NPCEncounter, and QuestStatus.
- RE: Data Grid control for inventory:
	"There are other columns types you can use to display 
		checkboxes (DataGridCheckBoxColumn), 
		dropdown boxes (DataGridComboBoxColumn), 
		text links (DataGridHyperlinkColumn), and 
		customized content (DataGridTemplateColumn)."
#	Movement & Navigation
##		Pace
Icon with picture of stance to select pace, similar to Intravenous.
Speed reduces AP costs but also chances of success. Also increases noise.
##		Audible Distance
All audible events have an audibility distance, which may need a somewhat complex algorithm to determine how they reach the player.
##		View Distance
All physical objects have a view distance, above which they cannot be seen when examining surroundings.
#	Cell Features
##	C	Gates
###		C	Blocking Movement
###		C	Inter-Cell Harmony
####		C	Gate Matching
A basement door should not match to a solid floor on the cell above
####		C	Rooftop Jumping
Fall damage, skill boni/mali, partial successes 
###		C	Skill Check Passage
##	C	Objects
###		C	Generic Objects
Don't go too into depth here. Objects in general should be treated as disposable. They're often evidence of crimes, or prohibited in certain areas.
It should stress the player character out to be holding evidence of their own crimes. Evidence of one crime is reasonable, but multiple can make you high strung and require a good amount of experience to keep cool.
###		C	Special Objects
####		C	Collection Call Sheet
#	NPCs
##	C	AI
###		C	Universal behaviors
####		C	Flight
Factors: Fear, Relation (e.g. more likely to run if they don't know/care who you are, it's not their fight)
##	C	Jobs & Jobgivers
###		C	Reputation Vicar (rename)
If your boss is scary, you become scarier. 
#	Scenes/Interactions
There should be no strict delineation between scenes. They are just divided here thematically in order to organize different features.
##	C	All Scenes
###		C	Advantage
A numerical representation of:
	How far ahead you are spatially in a chase
	How advantageous your position is in combat (particularly important when outnumbered)
	Possibly a victory condition for Dialogue (yet to see)
	How well-hidden you are in Stealth
###		C	Rate
From stealthy to frantic. Determine AP cost, noise, accuracy, etc.

Repertoire		Stealthy	Careful		Casual		Hurried		Frantic
	Combat			Reactive	Defensive	Balanced	Aggressive	Berserk
	Movement		Sneak		Cautious	Casual		Run			Sprint
	Speech			Observant	Insinuating	Level		Curt		Overbearing

###		C	Relationship
An Enum, defines NPC behavior
Important: can be declared by the player, leading to differences in interaction
##	C	Chase
###		C	Foot (Combat)
###		C	Foot (Tailing)
###		C	Vehicle (Combat)
###		C	Vehicle (Tailing)
##	C	Combat
###		C	Melee
###		C	Ranged
###		C	Unarmed
##	C	Dialogue
General measures
	Trust
	Fear
	Mood (light, heavy)
###		C	Collection
###		C	Employee
###		C	Employer
###		C	Fast-Talking the Cops
###		C	Interrogation
###		C	Scam
##	C	Stealth
###		C	Burglary
###		C	Guard Duty
Guard Duty does technically seem to fall into the stealth genre - lots of sense checks, and may include staying hidden
###		C	Holdout
Getting holdout items past searches and security
###		C	Losing a Tail
###		C	Vehicle Tailing
#	General Action Economy
##	C	Action Points
Spent by committing actions, each of which are subdivided by Windup, Execution, and Recovery
##	C	Action Types
Some actions allow a secondary action, like driving + shooting. I.e., it shouldn't slow your car down to shoot out the window. Obviously, mali would apply to both skills.
##	C	Initiative
- Rolled at start of scene, serving only to break tied events in the AP tracker. 
- Can change hands according to circumstances
- This actually sounds somewhat powerful, extending the range of playable actions that will allow you not to cede action to your opponent. E.g., now you can *choose* to tie APs because you'll still act first afterward.
###		C	General Scale
Before you set down any AP values, determine what the maximum and minimum ought to be. E.g., if you make "walk west" cost 4 AP, that doesn't leave much room for "drive 90mph west."
###		C	AP Schedule/Tracker
####		C	Committed Action half-transparency
Shows margin of 

##	C	Action Subdivision
Each action's is subdivided by APs into Setup, Execution, and Recovery. Interrupting the action during various points in the action will have different effects.
E.g.
	Roundhouse Kick (7AP)
		Setup (3 AP)
		Execution (2 AP)
		Recovery (2 AP)
##	C	Action Modifiers
E.g.
	Suckerfy
		Reduce Setup AP by 1
		Greatly reduced accuracy, which forces you to depend on an unaware target, making it only advantageous when throwing the first punch.
	Quick Recovery
		Reduce Recovery AP by 1
##	C	Action Configurator
###		C	Default Presets
Punch
	Strike with Readymost Limb at Vulnerablemost Vitals on Vulnerablemost Enemy
Headbutt
	Strike with Head at Head on Vulnerablemost Enemy
Mount
	Restrain with Readymost Leg at Matching Leg on Prone Enemy
###		C	Configuration Options
####		C	1.0 Action
####		C	1.1 Action Modifiers
#####			C	Combo
Overlaps Recovery and Setup of two different actions. Effects are synergistic.
Maybe this should be automatic, since a player would never *not* want to use it
E.g., Punch has automatic Combo with other punches 
#####			C	Suckerfy
#####			C	Haymaker
#####			C	Quick Recovery
####		C	2.0 Acting Bodypart
#####			Limb Type
Arm, Leg, Head, etc.
####		C	2.1 Acting Bodypart Modifiers
#####			C	Readymost 
#####			C	Strongest
###		C	Save as New Preset
#	OLD NOTES
## INSPIRATIONS
Seent:
	The Debt Collector (2018)
	Blood Simple (1984)
	Get Shorty (Series, 2017)
	Animal Kingdom
	Boardwalk Empire

To-watch:
	Uncut Gems (2019)
	Street Thief (2006)
	Midnight Run (1988)
	Barry (2018)
	Better Call Saul (2015 - )
	Peaky Blinders
## GAMEPLAY
Generally, you work on contract for various criminal organizations. 
Different map nodes are areas of town.

Jobs
	Collections
		You maintain a "Call Sheet" of clients who owe money. 
			Client name, description, location
			Threat rating
			Deadline
			Amount owed
			Commission
			Actions authorized (Need to visit employer to unlock new ones, i.e. authorization to kill debtor)
		You can make multiple visits to them to warn, threaten, beat them up, or kill them. Each has a deadline, before which you need to either collect or eliminate the debtor.
	Kidnapping
	Bounty Hunting
	Burglary
	Robbery
Scenes
	Close Combat
	Dialogue
	Foot Chase
	Ranged Combat
	Vehicle Chase

Score
	Cash is XP. Everytime you accrue a dollar, you gain that much XP.
		1. This is at least novel, even if strange and maybe not ideal
		2. May incentivize player to think like and therefore roleplay as a goon
## CHARACTERS
### Base Stats
Vitality - How to die
Endurance - How to pass out
Pain - How to go into pain shock
### Mannerisms
Each character has Mannerisms, generated from their attributes, which arise in specific conditions (Pain, Deception, Fear, etc. / Grimace, Smile, Frown, etc.). The player has to guess at what these mean, but occasionally the pattern will be revealed and tracked.
### Reputations
A lot of what you know about people is from reputation. It's hard to find a contract killer, so one of the best ways is to track rumors of someone who has killed before. It's very hard to simply read characteristics from people, though possible with enough experience and skill.
### Primary Attributes
	Height makes more vulnerable to body shots, side note
### Backgrounds
	Substantial skill and attribute boosts
	Only acquirable at game start
	Maybe layered for stages of life, might be interesting narratively. In this case you might want to give balancing advantages/disadvantages for youth vs. experience.
	Variable number of years in these phases, allowing customization
	Childhood (13-18 years)
		Street Kid
		Rich Kid
	Youth (18 - 30)
		
	Middle Age (30 - 50)

	Old Age (Can select multiple)
### Leveling Up
This shouldn't be as big in this game. Treat it like IRL experience: The character becomes cooler under pressure, knows what to look out for, but that's mostly it. But those are big advantages.
Leveling here might even be something like Oblivion (puke), where each skill levels independently with use. It most matches real life, and since all that is hidden from the player it shouldn't be cumbersome.

### Archetypes
Modify base attributes, by a percentage of the score rolled (not a percentage of the maximum possible score)
TODO: Add additional behaviors with Archetypes. They should feel like a different playstyle, akin to SOR classes. 
E.g., Schizophrenic occasionally has conversations with demons who berate and insult him

### Traits 
Available in Character Creation or later on.

- Bar Brawliste
  - Identify more improvised weaponry in Combat scenes.
- Criminally Mischievous 
  - Bonus to improvise weapons and obstacles (Gives advantage to react to them as well). Bonus to Intrusion and Vandalism.
- Keepin' it Warm
  - Improve your Holdout inventory size, even when you're not wearing clothes
- Eagle-Eyed
  - Cells & Gates etc. have their visibility distance increased by 1
- Myopic
  - Cells & Gates etc. have their visibility distance reduced by 1, to a minimum of 0.
- Off-Shrugger
  - You can choose not to dodge at a lower AP threshold than normal.
  - For context, dodging is an option that is not optional in split-second decisions. This reduces the field of what's considered split-second. 
- Pummeler Handerson 
  - Strike attacks use one less AP, or if you don't use AP they have some other bonus
- Rank Beginner
  - Start at Level 1, just a pathetic waif
- Steely-Eyed
  - You are less likely to Flinch at near-hits.
- Street Chiropractor
  - Extra chance to do permanent damage to the brain and spine... if you're into that.
- Thick Skull
  - Better headbutts, defense for head, lower intelligence
- Traceur 
  - Bonus to bypassing obstacles in chase scenes. Trains Breakfall in combat.
- Yarnspinner 
  - Your lies always succeed, unless your opponent has an Intel to the contrary.

### Statuses
- Off-Balance
  - Recover with Steady as a Free Action
- Staggered
  - Recover with Steady as a Main Action
  - Change to Off-Balance with Steady as a Free Action
- Stunned
- Prone
- Supine
- 
## FACTIONS
###		Prefab
####		Gold Star Specials
The mentally challenged, abandoned on the streets, have banded together for mutual protection. At some point, it took on a life of its own and now they rule some neighborhoods with force. Turns out, you don't have to be a genius to run a gang of thugs!
Their leader Dipshit has adopted that name to reclaim the names they've been called. His IQ is technically low, but since he's constantly underestimated, he tends to get the upper hand.
####		Sleepwalkers
DeepReal junkies, commanded to do the Oneiromancer's bidding, at pain of Mare!
A Mare (from Nightmare) is every conceivable torture drawn deep from the user's psyche, with all other Sleepwalkers forced to watch. But the Oneiromancer is not cruel, it only demands compliance. After the Mare, the disobedient one is 
## PHYSICAL ENVIRONMENT
Districts
	Districts are pools of Locations and associated percentage chances of encountering them.
	Types
		Downtown Streets
		Downtown Rooftops
		Downtown Subterrane (Subway & Sewers)
		Uptown Streets
		Uptown Rooftops
		Slum Streets
		Slum Rooftops
		City Park 
		Industrial Streets
		Industrial Rooftops
		Suburbia
		Countryside
Locations
	Types
		Dive Bar
		Fancy Bar
		Police Station
		Etc.
Sublocations
	Packaged in each Location, somewhat varied.
	Types
Objects
	Usable in various ways
	Examples
		Breaker Box (Sabotage opportunity)
		Beer Bottle (Improvised weapon)
		Suitcase full of cash
Gates
	Examples
		Chain Link Fence (chase scene stuff)
		Doorway (Can be open and shut. Gives bonus to blocking passage.)
		Alley gap (just air, but has special properties for jumping across, including elevation change calculation)
		Various walls or non-walls that categorically block passage
		
## GAMEPLAY
Attempt checks are based on various attributes and skills, in a complex web of contributions. Warn the player that stuff will do vaguely what it sounds like and they can ignore a lot of the details.

Rolling an Attempt
	Each contributing skill or attribute contributes a percentage of the roll - each of those is rolled to see if/how much of their bonus applies, rather than always applying a flat bonus.
	The advantage of this approach is that it can guide narration - not just THAT your punch failed, but WHY! By attaching the narration to a particular skill, you can be very specific and create a somewhat readable narrative of the encounter.
## SCENES

### FOOT CHASE
Chase or flee, make use of obstacles, or fight.

Action Economy
	Uses same as combat: Main Action, Secondary Action, Reaction.
	Action Types
		Run (Can be set to auto, or use NWSE keys)
		Climb
		Jump
		Hide from View (Run around a corner, etc., not stealth)
		Stealth (Actual hiding)
		Ambush (Requires out of view)
		Crawl
		Tackle
		Strike
		Shoot
		Create Obstacle (Tip shit over, slam doors, trip people, etc.)

Objects
	Trash Cans
	Fruit Vendor
	Etc.
	Gate Objects
		Semi-obstacles or actual obstacles gating one Subscene to another, or to a new Location. The default is *no* gate object, just an open street.
			Fenced Alley
			Ladder to Rooftops
			Sewer Drain

### CLOSE COMBAT
Highly focused on unarmed combat and improvised weaponry. Guns are very deadly and draw a lot of attention. They are a good way to get killed or arrested.

Statuses
	Prone
	Supine
	Ready
	Flat-footed
	Unaware
	Restrained (Per Limb)
		If Prone & any limb restrained, must un-restrain before standing
		Restraint requires own limbs
		Full mount = both legs restrained by both legs
	Dazed
	
Actions
	Types
		Strike
			Jab
				Can waste enemy guard
			Hook
				Best led with Jab
			Palm Strike
			Roundhouse Kick
				No Secondary
			Uppercut
				Bonus damage to head, abdomen and genitals
			Etc.
		Restrain
			Requires target limb, up to two
			Can't restrain legs with legs unless prone
			Requires own limbs used, up to two
		Guard
		Dodge
			Adds a bonus to next strike
			Slightly more difficult than Guard
		React (Defensive)
			Chooses best of Guard or Dodge, with a small penalty
		React (Offensive)
			Seeks to counterstrike
			Better chance of hit but gives them a swing at you
	Dynamics
		Some synergies, like following a Jab with a Hook 
		Some do not allow a secondary action
Secondary Actions
	Limited set of actions, like Guard and Dodge, or a poor Swing
	Allowed in addition to main action
	Can be added to action presets
	Best used with fail-proof actions like Guard
	At a slight penalty but free
Reactions
	Breakfall
	Tumble
	Block

Action Configurator
	Limb(s)
		Special options like "readymost" arm
	Target Limb(s)	
		Groupings like Vitals, which is head and torso
	Can include a secondary action to make a preset for the whole turn
	Examples, saved presets
		Box
			Strike enemy Vulnerablemost Vitals (Head & Torso only) with Readymost Arm; set least ready Arm to Guard
		Pummel
			Box, but secondary is also a strike
		Restrain
			Restrain enemy free limbs with best (calculated) limb
	Visual interface shows which actions are used up by the saved configuration (Main Action, Secondary Action, Reaction); or AP usage if you go with that.
Weaponry
	Improvised Weaponry
		There should be many occasions to show up unarmed - generally due to the rules of whatever place you're in. Improvising weaponry is a crucial skill.
		Character attribute for identifying possible weapons.

General Mechanics
	Near-misses still can have an effect - damage enemy's composure by scaring them or making them misstep or stagger.
		Reactive dodging will require a variable amount of AP, near-misses requiring 1AP to dodge for example. In effect, the lower tiers of success simply do AP damage to the target. 

### DIALOGUE
Since so many relationships in this game are contentious, dialogue will resemble combat a bit.

Using the typical Kissass/Hardass/Smartass style breakdown might be too broad for what you're going here. Those are the goal results of what you work toward in conversation, but they skip over the process of dialogue in a way that you won't here. Instead, the skills should be more granular, referring to single conversational maneuvers like: 
	Tell an Anecdote 
	Raise Stakes
	

Action Economy
	Vocal
	Facial
	Subtext
Action Types
	Initiate Combat (Option blocked if they have bodyguards, basically giving plot armor)
	Bluff
	Flatter
	Advise
	Scorn
	Insult
	Threaten
	Blackmail (Requires Intel, Documents, or assurance from employer that such is secured)
	Lie (Risks automatic failure if they have Intel on you)
Action Synergies
	
Action Preset Examples
	Just saying, for your safety
		Vocal: Advise
		Facial: Flatter
		Subtext: Blackmail
Character Attributes
	Advantage
		Determined by who holds the cards in the interaction. Maybe it comes down to Bodyguard count.
	Confidence
Character Statuses
	Various emotional states affect AI and player action options
	E.g., if you're Angry, it's harder to keep your voice different from your subtext
Relationship Measures
	Dynamics
		All can be asymmetrical, as people disagree on the relationship.
		
	Dominance
		Both high: Aggressive competition
		Both low: Deep love or friendship
		Both medium: Cooperative but independent, mutual respect
		High/Low: Subjugation
	Harmony
		Composite score, a measure of how the other measures match each other. Call it an Understanding - crucial for a good working or romantic relationship.	
		Low harmony makes it more likely for disagreements (random events) in conversation to pop up.
	Trust

### SHOOTOUT
Shootouts should be very deadly and generally avoided. Character can only be viable in a shootout through reaction, surprise, experience or training.

Cover
	Might be able to copy Scary Guns data here
Movement
	Movement scale is zoomed out for these scenes - characters are moving to different areas of the building.
	Can use NWSE buttons in combat here. If you exit the shootout setpiece it turns into a Chase Scene.
## OBJECTS
Some objects will serve multiple purposes. This means either you have a big generic class, or use interfaces for their available functions.

### Physical Features

#### Gates

#### Obstacles

#### Scenery
Furniture you can throw or do whatever else to

### PhysicalItems


# Location Generation Hierarchy
Map
 District
  Block
   Chunk
	Gate
    Cell
     FeatureGroup
      Feature
       ItemGroup
        Item

# Narration system
Many objects will have Narrators attached - storage for various modifiers for the text that will appear. They should communicate with each other, e.g. Actor giving information that colors how MapCell introduces itself.