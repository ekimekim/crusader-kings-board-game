
import logging
import random


class Serializable:
	SERIALIZED_FIELDS = []

	def encode(self):
		return {
			attr: getattr(self, attr)
			for attr in self.SERIALIZED_FIELDS
		}


class Game(Serializable):
	MAP_DATA_FILE = "./map.json"

	SERIALIZED_FIELDS = [
		"realm_deck",
		"intrigue_deck",
		"war_deck",
		"tax_deck",
		"crusade_deck",
		"development_deck",
		"developments",
		"crusades",
		"map",
		"players",
	]

	def __init__(self, players, scenario_setup):
		"""players: {color: "human" | "ai"}"""
		self.log = logging.getLogger("game")

		self.realm_deck = Deck([])
		self.intrigue_deck = Deck([])
		self.war_deck = Deck([])
		self.tax_deck = Deck([])
		self.crusade_deck = Deck([])
		self.development_deck = Deck([
			Development("councilor", "Court Physician", None)
		])
		self.developments = []

		self.crusades = [
			Crusade("Constantinople", "Ignorant", None),
		]

		self.map = {}
		with open(self.MAP_DATA_FILE) as f:
			map_data = json.load(f)
		for name, culture in map_data["cultures"].items():
			if culture not in players:
				continue
			edges = (
				set(a for a, b in map_data["edges"] if b == name)
				| set(b for a, b in map_data["edges"] if a == name)
			)
			seas = set(sea for sea, regions in map_data["seas"].items() if name in regions)
			self.map[name] = Region(name, culture, edges, seas)

		self.players = {
			color: Player(color, kind)
			for color, kind in players.items()
		}

		scenario_setup(self)


class Region(Serializable):
	SERIALIZED_FIELDS = [
		"name",
		"culture",
		"edges",
		"seas",
		"owner",
		"castle",
		"mobilized",
		"duke",
		"tokens",
	]

	def __init__(self, name, culture, edges, seas):
		self.name = name
		self.culture = culture
		self.edges = edges
		self.seas = seas
		# owner is one of: player name, Pact(player name), Character (ie. independent)
		self.owner = Character.generate(self.culture)
		self.castle = False
		self.mobilized = False
		self.duke = None
		self.tokens = []


class Crusade(Serializable):
	SERIALIZED_FIELDS = ["name", "owner", "trait", "bonus"]

	def __init__(self, name, trait, bonus):
		self.name = name
		self.owner = None
		self.trait = trait
		self.bonus = bonus


class Deck(Serializable):
	SERIALIZED_FIELDS = ["items", "discards"]

	def __init__(self, items):
		self.items = items
		self.discards = []
		self.shuffle()

	def draw(self):
		if not self.items:
			self.items = self.discards
			self.discards = []
			self.shuffle()
		if not self.items:
			return None
		return self.items.pop(0)

	def discard(self, card):
		self.discards.append(card)

	def shuffle(self):
		random.shuffle(self.items)

	def find(self, **criteria):
		for card in self.items:
			if all(
				getattr(card, key) == value
				for key, value in criteria.items()
			):
				self.items.remove(card)
				return card
		return None

	def must_find(self, **criteria):
		card = self.find(**criteria)
		if card is None:
			raise ValueError("Could not find card with: {}".format(criteria))
		return card


class Development(Serializable):
	SERIALIZED_FIELDS = ["kind", "name", "bonus"]

	def __init__(self, kind, name, bonus):
		self.kind = kind
		self.name = name
		self.bonus = bonus


class Action(Serializable):
	SERIALIZED_FIELDS = ["kind", "name", "event"]

	def __init__(self, kind, name, event):
		self.kind = kind
		self.name = name
		self.event = event


class Player(Serializable):
	SERIALIZED_FIELDS = [
		"color",
		"kind",
		"gold",
		"traits",
		"hand",
		"innovations",
		"councilors",
		"ruler",
		"siblings",
		"children",
	]
	def __init__(self, color, kind):
		self.color = color
		self.kind = kind
		self.gold = 0
		self.traits = []
		self.hand = []
		self.innovations = []
		self.councilors = []
		self.ruler = None
		self.siblings = []
		self.children = []

	def add_development(self, card):
		if card.kind == "innovation":
			l = self.innovations
		elif card.kind == "councilor":
			l = self.councilors
		else:
			assert False
		# TODO discard if too many
		if len(l) >= 3:
			raise NotImplementedError
		l.append(card)


class Character(Serializable):
	SERIALIZED_FIELDS = [
		"gender",
		"name",
		"id",
		"traits",
		"spouse",
	]

	CULTURE_NAMES = {
		"black": [
			"Gertrude",
		],
	}

	RANDOM_TRAITS = [
		"Ignorant",
	]

	def __init__(self, gender, name, traits):
		self.gender = gender
		self.name = name
		self.id = random.randrange(100)
		self.traits = traits
		self.spouse = None

	@classmethod
	def generate(cls, culture):
		return Character(
			gender = random.choice(["male", "female"]),
			name = random.choice(cls.CULTURE_NAMES[culture]),
			traits = random.choice(cls.RANDOM_TRAITS),
		)


def generic_setup(game, data):
	for color, player_data in data.items():
		if color not in game.players:
			continue
		player = game.players[color]
		player.ruler = Character(**player_data["ruler"])
		player.gold = player_data["gold"]
		for development in player_data["developments"]:
			card = game.development_deck.must_find(name=development)
			player.add_development(card)
		for name in player_data["regions"]:
			region = game.map[name]
			region.owner = color
			if name in player_data["castles"]:
				region.castle = True


def first_crusade(game):
	generic_setup(game, {
		"red": {
			"ruler": {
				"gender": "male",
				"name": "William the Conqueror",
				"traits": ["Pious", "Brave", "Cruel", "Dimwitted"],
			},
			"regions": ["Wessex", "Northumbria", "Mercia", "Normandy"],
			"castles": [],
			"gold": 5,
			"developments": ["Navy"],
		},
		"black": {
			"ruler": {
				"gender": "male",
				"name": "Heinrich IV",
				"traits": ["Cultivated", "Clever", "Ambitious", "Cruel"],
			},
			"regions": ["Saxony", "Pomerania", "Poland", "Bohemia"],
			"gold": 5,
			"developments": ["Court Physician"],
			"castles": [],
		},
	})


def default(obj):
	if isinstance(obj, Serializable):
		return obj.encode()
	if isinstance(obj, set):
		return list(obj)
	else:
		raise TypeError("{!r} is not serializable".format(obj))


if __name__ == '__main__':
	import json
	game = Game({"black": "human"}, first_crusade)
	data = game.encode()
	print(json.dumps(data, default=default))
