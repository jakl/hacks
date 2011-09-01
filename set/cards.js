var cards = {
  init:function (){
    this.deck = [];
    for(var number = 0; number <=2; number++)
    for(var symbol = 0; symbol <=2; symbol++)
    for(var shading = 0; shading <=2; shading++)
    for(var color = 0; color <=2; color++)
      this.deck.push(new card({number:number,symbol:symbol,shading:shading,color:color}));
  },
  paint:function(){
    deck[Math.random()*deck.length].location = 'play';
  },
  paint:function(g){
    var board = [];
    for(var card in this.deck)
      if(card.location == 'play')
        board.push(card);
    card.width = 1/3;
    card.height = 1/4;
    var i = 0;
    for(var x = 0; x < 3; x++)
      for(var y = 0; y < 4; y++)
        board[i++].paint(g,x/3,y/4);
  },
}
