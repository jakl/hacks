//number   one, two, three
//symbol   diamond, squiggle, oval
//shading  solid, striped, open
//color    red, green, purple
function card(params){
  this.number=new number(params.number);
  this.symbol=new symbol(params.symbol);
  this.shading=new shading(params.shading);
  this.color=new color(params.color);
  this.x=params.x;
  this.y=params.y;
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
    return card.number.equals(this.number) && card.symbol.equals(this.symbol) && card.shading.equals(this.shading) && card.color.equals(this.color);
  },
  draw:function(g){
    var x = this.x;
    var y = this.y;
    if(this.shading.string == 'striped')
      switch(this.color.string){
        case 'red':g.fillStyle = g.strokeStyle = card.red_stripe; break;
        case 'green':g.fillStyle = g.strokeStyle = card.green_stripe; break;
        case 'purple':g.fillStyle = g.strokeStyle = card.purple_stripe; break;
      }
    else
      switch(this.color.string){
        case 'red':g.fillStyle = g.strokeStyle = '#f00'; break;
        case 'green':g.fillStyle = g.strokeStyle = '#0f0'; break;
        case 'purple':g.fillStyle = g.strokeStyle = '#00f'; break;
      }
    switch(this.number.string){
      case 'one':this.draw_symbol(g,x,y,card.width,card.height); break;
      case 'two':
        this.draw_symbol(g,x,y,card.width,card.height/2);
        this.draw_symbol(g,x,y+card.height/2,card.width,card.height/2);
        break;
      case 'three':
        this.draw_symbol(g,x,y,card.width,card.height/3);
        this.draw_symbol(g,x,y+card.height/3,card.width,card.height/3);
        this.draw_symbol(g,x,y+card.height*2/3,card.width,card.height/3);
        break;
    }
  },
  draw_symbol:function(g,x,y,w,h){
    var striped = this.shading.string == 'striped';
    switch(this.symbol.string){
      case 'diamond': this.draw_diamond(g,x,y,w,h); break;
      case 'squiggle': this.draw_squiggle(g,x,y,w,h); break;
      case 'oval': this.draw_oval(g,x,y,w,h); break;
    }
  },
  draw_diamond:function(g,x,y,w,h){
    x=g.canvas.width*x;
    y=g.canvas.height*y;
    w=g.canvas.width*w;
    h=g.canvas.height*h;
    g.beginPath();
    g.moveTo(x,h/2+y);
    g.lineTo(w/2+x,y);
    g.lineTo(w+x,h/2+y);
    g.lineTo(w/2+x,h+y);
    g.lineTo(x,h/2+y);
    g.closePath();
    this.shade(g);
  },
  draw_oval:function(g,x,y,w,h){
    w=g.canvas.width*w;
    h=g.canvas.height*h;
    x=g.canvas.width*x;
    y=g.canvas.height*y;
    g.beginPath();

    var kappa = .5522848;
    ox = (w / 2) * kappa; // control point offset horizontal
    oy = (h / 2) * kappa; // control point offset vertical
    xe = x + w;           // x-end
    ye = y + h;           // y-end
    xm = x + w / 2;       // x-middle
    ym = y + h / 2;       // y-middle

    g.beginPath();
    g.moveTo(x, ym);
    g.bezierCurveTo(x, ym - oy, xm - ox, y, xm, y);
    g.bezierCurveTo(xm + ox, y, xe, ym - oy, xe, ym);
    g.bezierCurveTo(xe, ym + oy, xm + ox, ye, xm, ye);
    g.bezierCurveTo(xm - ox, ye, x, ym + oy, x, ym);
    g.closePath();
    this.shade(g);
  },
  draw_squiggle:function(g,x,y,w,h){
    x=g.canvas.width*x;
    y=g.canvas.height*y;
    w=g.canvas.width*w;
    h=g.canvas.height*h;

    g.beginPath();
    g.moveTo(w+x,h/2-h/5+y);
    g.bezierCurveTo(w/2+x,-h/2-h/5+y,w/2+x,h*3/2-h/5+y,x,h/2-h/5+y);
    g.lineTo(x,h/2+h/5+y);
    g.bezierCurveTo(w/2+x,h*3/2+h/5+y,w/2+x,-h/2+h/5+y,w+x,h/2+h/5+y);
    g.lineTo(w+x,h/2-h/5+y);
    this.shade(g);
  },
  shade:function(g){
    switch(this.shading.string){
      case 'solid':g.fill(); break;
      case 'striped':g.fill(); break;
      case 'open':g.stroke(); break;
    }
  },
});
add_statics(card,{
  width:1/4,height:1/3,
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
