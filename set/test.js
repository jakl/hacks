test('Card Mechanics', function(){
  equals(card.get_third_symbol('diamond','diamond'),'diamond', 'Two diamonds need a third diamond to complete the set');
  equals(card.get_third_symbol('squiggle','squiggle'),'squiggle', 'Two squiggles need a third squiggle to complete the set');
  equals(card.get_third_symbol('oval','oval'),'oval', 'Two ovals need a third oval to complete the set');
  equals(card.get_third_symbol('diamond','squiggle'),'oval', 'Diamond + squiggle = oval');
  equals(card.get_third_symbol('diamond','oval'),'squiggle', 'Diamond + oval = squiggle');
  equals(card.get_third_symbol('squiggle','oval'),'diamond', 'Squiggle + oval = diamond');
  equals(card.get_third_color('red','purple'),'green', 'Red + purple = green');
  equals(card.get_third_number(2,2),2, '2 + 2 = 2');
  equals(card.get_third_shading('open','solid'),'striped', 'Open + solid = striped');
  var c1 = new card(1,'diamond','solid','red');
  var c2 = new card(2,'squiggle','striped','green');
  var c3 = new card(3,'oval','open','purple');
  var c3_test = c1.get_third_card(c2);
  ok(c3_test.equals(c3), 'Cards can find their third mate for a set');
});
//number one, two, three
//symbol diamond, squiggle, oval
//shading solid, striped, open
//color red, green, purple
