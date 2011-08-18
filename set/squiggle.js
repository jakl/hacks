var main = {
  resize:function(){
    main.g.canvas.width = window.innerWidth*.95;
    main.g.canvas.height = window.innerHeight*.95;
    main.paint();
  },
  init:function(){
    this.g = document.getElementById('canvas').getContext('2d');
    window.onresize=this.resize;
    this.resize();
  },
  paint:function(){
    var g = this.g;
    g.fillStyle = '#000';
    g.fillRect(0,0,this.g.canvas.width,this.g.canvas.height);
    var w = this.g.canvas.width;
    var h = this.g.canvas.height;
    var x = 0;
    var y = 0;

    g.fillStyle = '#444';
    g.strokeStyle = '#444';

//vertical
    g.beginPath();
    g.lineWidth = 10;
    g.moveTo(w/2-w/5+x,y);
    g.bezierCurveTo(-w/2-w/5+x,h/2+y,w*3/2-w/5+x,h/2+y,w/2-w/5+x,h+y);
    g.lineTo(w/2+w/5+x,h+y);
    g.bezierCurveTo(w*3/2+w/5+x,h/2+y,-w/2+w/5+x,h/2+y,w/2+w/5+x,y);
    g.lineTo(w/2-w/5+x,y);
    g.stroke();

//horizontal
    g.lineWidth = 20;
    g.beginPath();
    g.moveTo(w+x,h/2-h/5+y);
    g.bezierCurveTo(w/2+x,-h/2-h/5+y,w/2+x,h*3/2-h/5+y,x,h/2-h/5+y);
    g.lineTo(x,h/2+h/5+y);
    g.bezierCurveTo(w/2+x,h*3/2+h/5+y,w/2+x,-h/2+h/5+y,w+x,h/2+h/5+y);
    g.lineTo(w+x,h/2-h/5+y);
    g.stroke();
  },
}
