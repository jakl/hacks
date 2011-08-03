//number one, two, three
//symbol diamond, squiggle, oval
//shading solid, striped, open
//color red, green, purple
function card(number,symbol,shading,color){
  this.number=number;
  this.symbol=symbol;
  this.shading=shading;
  this.color=color;
}
set_member_properties(card,{
  get_third_card:function(x){
    return new card(
      this.get_third_number(x.number),
      this.get_third_symbol(x.symbol),
      this.get_third_shading(x.shading),
      this.get_third_color(x.color));
  },
  equals:function(card){
    return card.number==this.number && card.symbol==this.symbol && card.shading==this.shading && card.color==this.color;
  },
  get_third_number:function(x){
    return card.get_third_number(this.number,x);
  },
  get_third_symbol:function(x){
    return card.get_third_symbol(this.symbol,x);
  },
  get_third_shading:function(x){
    return card.get_third_shading(this.shading,x);
  },
  get_third_color:function(x){
    return card.get_third_color(this.color,x);
  },
});
set_static_properties(card,{
  number:{one:1,two:2,three:3},
  symbol:{diamond:1,squiggle:2,oval:3},
  shading:{solid:1,striped:2,open:3},
  color:{red:1,green:2,purple:3},
  get_third:function(a,b){
    if(a==b)
      return a;
    if((a==2 || b==2) && (a==3 || b==3))
      return 1;
    if((a==3 || b==3) && (a==1 || b==1))
      return 2;
    if((a==1 || b==1) && (a==2 || b==2))
      return 3;
  },
  get_third_symbol:function(a,b){
    if(a==b)
      return a;
    if((a=='squiggle' || b=='squiggle') && (a=='oval' || b=='oval'))
      return 'diamond';
    if((a=='oval' || b=='oval') && (a=='diamond' || b=='diamond'))
      return 'squiggle';
    if((a=='diamond' || b=='diamond') && (a=='squiggle' || b=='squiggle'))
      return 'oval';
  },
  get_third_shading:function(a,b){
    if(a==b)
      return a;
    if((a=='striped' || b=='striped') && (a=='open' || b=='open'))
      return 'solid';
    if((a=='open' || b=='open') && (a=='solid' || b=='solid'))
      return 'striped';
    if((a=='solid' || b=='solid') && (a=='striped' || b=='striped'))
      return 'open';
  },
  get_third_color:function(a,b){
    if(a==b)
      return a;
    if((a=='green' || b=='green') && (a=='purple' || b=='purple'))
      return 'red';
    if((a=='purple' || b=='purple') && (a=='red' || b=='red'))
      return 'green';
    if((a=='red' || b=='red') && (a=='green' || b=='green'))
      return 'purple';
  },
});
