

class Serializable:
	SERIALIZED_FIELDS = []

	def encode(self):
		data = {}
		for attr in SERIALIZED_FIELDS:
			value = getattr(self, attr)
			if isinstance(value, Serializable):
				value = value.encode()
			data[attr] = value
		return data

	@classmethod
	def decode(cls, data):
		self = cls.__new__(cls)
		for attr in SERIALIZED_FIELDS:
			value = data[attr]
			setattr(self, attr, data[attr])
		return self


class Game(Serializable):
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

	SERIALIZED_FIELDS = {
		"realm_deck": ActionDeck,
		"intrigue_deck": ActionDeck,
		"war_deck": ActionDeck,
		"tax_deck": ActionDeck,
		"crusade_deck": ActionDeck,
		"development_deck": DevelopmentDeck,
		"developments": [Development],
		"crusades": [Crusade]
		"map": {str: Region},
		"players": {str: Player},
	}

	def __init__(self, players, scenario_setup):
		"""players: {color: "human" | "ai"}"""
		self.log = logging.getLogger("game")

		self.realm_deck = Deck([])
		self.intrigue_deck = Deck([])
		self.war_deck = Deck([])
		self.tax_deck = Deck([])
		self.crusade_deck = Deck([])
		self.development_deck = Deck([])
		self.developments = []

		self.crusades = [
			Crusade("Constantinople", "Ignorant", Bonus.CrusadeCard),
		]

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


class Region(Serializable):
	SERIALIZED_FIELDS = {
		name: str,
		culture: str,
		edges: [str],
		seas: [str],
		owner:
	}

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


class Crusade(Serializable):
	def __init__(self, name, trait, bonus):
		self.name = name
		self.owner = None
		self.trait = trait
		self.bonus = bonus


class Deck:
	def __init__(self, items):
		self.items = items
		self.discards = []


class ActionDeck(Deck, Serializable):
	SERIALIZED_FIELDS = {
		"items": [Action],
		"discard": [Action],
	}


class DevelopmentDeck(Deck, Serializable):
	SERIALIZED_FIELDS = {
		"items": [Development],
		"discard": [Development],
	}


class Development(Serializable):
	def __init__(self, kind, name, bonus):
		self.kind = kind
		self.name = name
		self.bonus = bonus


class Action(Serializable):
	def __init__(self, kind, name, event):
		self.kind = kind
		self.name = name
		self.event = event


class Player(Serializable):
	def __init__(self, color):
		self.color = color
		self.gold = 0
		self.traits = []
		self.hand = []
		self.innovations = []
		self.councilors = []
		self.ruler = None
		self.siblings = []
		self.children = []


class Character(Serializable):
	def __init__(self, gender, name, id, traits):
		self.gender = gender
		self.name = name
		self.id = id
		self.traits = traits
		self.spouse = None
