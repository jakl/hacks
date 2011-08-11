function number(value){
  this.__defineSetter__('value',function(val){
    switch(val){
      case 'one': case 0: this.m_value = 0; break;
      case 'two': case 1: this.m_value = 1; break;
      case 'three': case 2: this.m_value = 2; break;
      default:throw "invalid value ("+val+") must be any of: one two three OR 0 1 2";
    }
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
add_statics(number,{
  offset:0,
  equals:function(x){return this.value == x.value},
});
add_members(number,{get_third:function(x){return new number(card.get_third(this.value,x.value));}});

function color(value){
  this.__defineSetter__('value',function(val){
    switch(val){
      case 'red': case 0: this.m_value = 0; break;
      case 'green': case 1: this.m_value = 1; break;
      case 'purple': case 2: this.m_value = 2; break;
      default:throw "invalid value ("+val+") must be any of: red green purple OR 0 1 2";
    }
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
add_statics(color,{
  offset:0,
  equals:function(x){return this.value == x.value},
});
add_members(color,{get_third:function(x){return new color(card.get_third(this.value,x.value));}});

function shading(value){
  this.__defineSetter__('value',function(val){
    switch(val){
      case 'solid': case 0: this.m_value = 0; break;
      case 'striped': case 1: this.m_value = 1; break;
      case 'open': case 2: this.m_value = 2; break;
      default:throw "invalid value ("+val+") must be any of: solid striped open OR 0 1 2";
    }
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
add_statics(shading,{
  offset:0,
  equals:function(x){return this.value == x.value},
});
add_members(shading,{get_third:function(x){return new shading(card.get_third(this.value,x.value));}});

function symbol(value){
  this.__defineSetter__('value',function(val){
    switch(val){
      case 'diamond': case 0: this.m_value = 0; break;
      case 'squiggle': case 1: this.m_value = 1; break;
      case 'oval': case 2: this.m_value = 2; break;
      default:throw "invalid value ("+val+") must be any of: diamond squiggle oval OR 0 1 2";
    }
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
add_statics(symbol,{
  offset:0,
  equals:function(x){return this.value == x.value},
});
add_members(symbol,{get_third:function(x){return new symbol(card.get_third(this.value,x.value));}});
