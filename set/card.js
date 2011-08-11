//number   one, two, three
//symbol   diamond, squiggle, oval
//shading  solid, striped, open
//color    red, green, purple
function card(number_value,symbol_value,shading_value,color_value){
  this.number=new number(number_value);
  this.symbol=new symbol(symbol_value);
  this.shading=new shading(shading_value);
  this.color=new color(color_value);
};
add_members(card,{
  rotate_features:function(){
    var tmp_color = this.color.value;
    this.color.value = this.shading.value;
    this.shading.value = this.symbol.value;
    this.symbol.value = this.number.value;
    this.number.value = tmp_color;
  },
  get_third_card:function(x){
    return new card(
      card.get_third(this.number.value,x.number.value),
      card.get_third(this.symbol.value,x.symbol.value),
      card.get_third(this.shading.value,x.shading.value),
      card.get_third(this.color.value,x.color.value));
  },
  equals:function(card){
    return card.number.value==this.number.value && card.symbol.value==this.symbol.value && card.shading.value==this.shading.value && card.color.value==this.color.value;
  },
});
add_statics(card,{
  increment_offsets:function(){number.offset++;symbol.offset++;shading.offset++;color.offset++;},
  get_third:function(a,b){
    if(a==b)
      return a;
    if((a==1 || b==1) && (a==2 || b==2))
      return 0;
    if((a==2 || b==2) && (a==0 || b==0))
      return 1;
    if((a==0 || b==0) && (a==1 || b==1))
      return 2;
  },
});




function number(value){
  this.__defineSetter__('value',function(val){
    if(val == 'one') this.m_value = 0;
    else if(val == 'two') this.m_value = 1;
    else if(val == 'three') this.m_value = 2;
    else if(val >= 0 && val <=2) this.m_value = val;
    else throw "invalid value ("+val+") must be any of: one two three OR 0 1 2";
  });
  this.__defineGetter__('value',function(){return (this.m_value+number.offset)%3});
  this.__defineGetter__('string',function(){
    switch(this.value){
      case 0:return 'one';
      case 1:return 'two';
      case 2:return 'three';
      default:throw 'bad number ' + this.val;
  }});
  this.value = value;
};
add_statics(number,{offset:0});
add_members(number,{get_third:function(x){return new number(card.get_third(this.value,x.value));}});

function color(value){
  this.__defineSetter__('value',function(val){
    if(val == 'red') this.m_value = 0;
    else if(val == 'green') this.m_value = 1;
    else if(val == 'purple') this.m_value = 2;
    else if(val >= 0 && val <=2) this.m_value = val;
    else throw "invalid value ("+val+") must be any of: red green purple OR 0 1 2";
  });
  this.__defineGetter__('value',function(){return (this.m_value+color.offset)%3});
  this.__defineGetter__('string',function(){
    switch(this.value){
      case 0:return 'red';
      case 1:return 'green';
      case 2:return 'purple';
      default:throw 'bad color ' + this.val;
  }});
  this.value = value;
};
add_statics(color,{offset:0});
add_members(color,{get_third:function(x){return new color(card.get_third(this.value,x.value));}});

function shading(value){
  this.__defineSetter__('value',function(val){
    if(val == 'solid') this.m_value = 0;
    else if(val == 'striped') this.m_value = 1;
    else if(val == 'open') this.m_value = 2;
    else if(val >= 0 && val <=2) this.m_value = val;
    else throw "invalid value ("+val+") must be any of: solid striped open OR 0 1 2";
  });
  this.__defineGetter__('value',function(){return (this.m_value+shading.offset)%3});
  this.__defineGetter__('string',function(){
    switch(this.value){
      case 0:return 'solid';
      case 1:return 'striped';
      case 2:return 'open';
      default:throw 'bad shading ' + this.val;
  }});
  this.value = value;
};
add_statics(shading,{offset:0});
add_members(shading,{get_third:function(x){return new shading(card.get_third(this.value,x.value));}});

function symbol(value){
  this.__defineSetter__('value',function(val){
    if(val == 'diamond') this.m_value = 0;
    else if(val == 'squiggle') this.m_value = 1;
    else if(val == 'oval') this.m_value = 2;
    else if(val >= 0 && val <=2) this.m_value = val;
    else throw "invalid value ("+val+") must be any of: diamond squiggle oval OR 0 1 2";
  });
  this.__defineGetter__('value',function(){return (this.m_value+symbol.offset)%3});
  this.__defineGetter__('string',function(){
    switch(this.value){
      case 0:return 'diamond';
      case 1:return 'squiggle';
      case 2:return 'oval';
      default:throw 'bad symbol ' + this.val;
  }});
  this.value = value;
};
add_statics(symbol,{offset:0});
add_members(symbol,{get_third:function(x){return new symbol(card.get_third(this.value,x.value));}});
