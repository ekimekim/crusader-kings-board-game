

class Game:
	REGION_CULTURES = {
		"Hungary": "black",
		"Saxony": "black",
	}

	REGION_EDGES = [
		("Hungary", "Saxony"),
	]

	SEAS = {
		"Test Sea": ("Hungary", "Saxony"),
	}

	def __init__(self, players, scenario_setup):
		"""players: {color: "human" | "ai"}"""
		self.log = logging.getLogger("game")

		self.realm_deck = [] # Action[]
		self.intrigue_deck = [] # Action[]
		self.war_deck = [] # Action[]
		self.tax_deck = [] # Action[]
		self.crusade_deck = [] # Action[]

		self.development_deck = [] # Development[]

		self.crusades = [] # Crusade[]

		self.map = {}
		for name, culture in cultures.items():
			if culture not in players:
				continue
			edges = set(a for a, b in edges if b == name) |
			        set(b for a, b in edges if a == name)
			seas = set(sea for sea, regions in self.seas if name in regions)
			self.map[name] = Region(name, culture, edges, seas)

		self.players = {
			color: Player(color, kind)
			for color, kind in players.items()
		}

		scenario_setup(self)


class Region:
	def __init__(self, name, culture, edges, seas):
		self.name = name
		self.culture = culture
		self.edges = edges
		self.seas = seas
		# owner is one of: player name, Pact(player name), Character (ie. independent)
		self.owner = Character(self.culture)
		self.castle = False
		self.mobilized = False
		self.duke = None
		self.tokens = []


class Crusade:
	def __init__(self, trait, bonus):
		self.owner = None
		self.trait = trait
		self. 
