function draw_tiles(centerx, centery){
  if(this.vieww < 2)
    this.vieww = 2;
  if(this.viewh < 2)
    this.viewh = 2;
  var vieww = this.vieww;
  var viewh = this.viewh;
  var tile_width = w/vieww;
  var tile_height = h/viewh;
  for(x=centerx-vieww/2;x<centerx+vieww/2;x++)
    for(y=centery-viewh/2;y<centery+viewh/2;y++)
      try{
        g.drawImage(this.sprites[this.tiles[x][y].type],(x-(centerx-vieww/2))*tile_width,(y-(centery-viewh/2))*tile_height,tile_width,tile_height);
      } catch(e){}
  g.drawImage(this.sprites.trainer,w/2,h/2,tile_width,tile_height);
}

function init_tiles(){
  this.tiles = new Array(this.width);
  for(i=0;i<this.width;i++)
    this.tiles[i] = new Array(this.height);
    
  for(i=0;i<this.width;i++)
    for(j=0;j<this.height;j++){
      var rand = Math.random();
      if(rand > .8)
        this.tiles[i][j] = {type:"grass",passable:true};
      else if(rand > .6)
        this.tiles[i][j] = {type:"lava",passable:false};
      else if(rand > .4)
        this.tiles[i][j] = {type:"water",passable:false};
      else
        this.tiles[i][j] = {type:"cave",passable:true};
    }
}

function getImage(name){
  var tmp = new Image();
  tmp.src=name;
  return tmp;
}
