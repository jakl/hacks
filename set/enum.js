//NOT USED - BROKEN SAMPLE CODE FROM JAVASCRIPT: THE DEFINITIVE GUIDE
function enum(namesToValues){
  var enum = function() {throw "Can't instantiate enums"};
  var proto = enum.prototype = {
    contructor: enum,
    toString: function(){return this.name},
    valueOf: function(){return this.value},
    toJSON: function(){return this.name;},
  };

  enum.values = [];

  for(name in namesToValues){
    var e = Object.create(proto);
    e.name=name;
    e.value=namesToValues[name];
    enum[name] = e;
    enum.values.push(e);
  }

  enum.foreach = function(f,c){
    for(var i=0; i<this.values.length; i++)
      f.call(c,this.values[i]);
  }
  return enum;
}
function Card(suit,rank){
  this.suit = suit;
  this.rank = rank;
  Card.prototype.toString=function(){
    return this.rank.toString() + " of " + this.suit.toString();
  }
}

Card.Suit = enum({a:0,b:1,c:2});
Card.Rank = enum({x:0,y:1,z:2});

// Compare the value of two cards as you would in poker
Card.prototype.compareTo = function(that) {
  if (this.rank < that.rank) return -1;
  if (this.rank > that.rank) return 1;
  return 0;
};

Card.orderByRank = function(a,b) { return a.compareTo(b); };

function Deck() {
  var cards = this.cards = [];
  // A deck is just an array of cards
  Card.Suit.foreach(function(s) { // Initialize the array
    Card.Rank.foreach(function(r) {
      cards.push(new Card(s,r));
    });
  });
}
Deck.prototype.shuffle = function() {
  // For each element in the array, swap with a randomly
  var deck = this.cards, len = deck.length;
  for(var i = len-1; i > 0; i--) {
    var r = Math.floor(Math.random()*(i+1)), temp;
    temp = deck[i], deck[i] = deck[r], deck[r] = temp;
  }
  return this;
};
// Deal method: returns an array of cards
Deck.prototype.deal = function(n) {
  if (this.cards.length < n) throw "Out of cards";
  return this.cards.splice(this.cards.length-n, n);
};
console.log(Card);
// Create a new deck of cards, shuffle it, and deal a hand
var deck = (new Deck()).shuffle();
var hand = deck.deal(13).sort(Card.orderByRank);
