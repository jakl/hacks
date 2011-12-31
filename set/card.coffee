#number   one, two, three
#symbol   diamond, squiggle, oval
#shading  solid, striped, open
#color    red, green, purple
@module 'set',->
  class card {
    constructor: (params) ->
      @number=new number(params.number)
      @symbol=new symbol(params.symbol)
      @shading=new shading(params.shading)
      @color=new color(params.color)
      @x=params.x
      @y=params.y
      @location=params.location

    rotate_features: ->
      tmp_color = @color.value
      @color.value = @shading.value
      @shading.value = @symbol.value
      @symbol.value = @number.value
      @number.value = tmp_color

    get_third_card: (x) ->
      return new card(
        card.get_third(@number.value,x.number.value),
        card.get_third(@symbol.value,x.symbol.value),
        card.get_third(@shading.value,x.shading.value),
        card.get_third(@color.value,x.color.value))

    equals: (card) ->
      return card.number.equals(@number) && card.symbol.equals(@symbol) && card.shading.equals(@shading) && card.color.equals(@color)

    paint: (g,x,y) ->
      x = @x || x
      y = @y || y
      if @shading.string == 'striped'
        switch @color.string
            when 'red' then g.fillStyle = g.strokeStyle = card.red_stripe break
            when 'green' then g.fillStyle = g.strokeStyle = card.green_stripe break
            when 'purple' then g.fillStyle = g.strokeStyle = card.purple_stripe break
      else
        switch @color.string
          when 'red' then g.fillStyle = g.strokeStyle = '#f00' break
          when 'green' then g.fillStyle = g.strokeStyle = '#0f0' break
          when 'purple' then g.fillStyle = g.strokeStyle = '#00f' break
      switch @number.string
        when 'one' then @paint_symbol(g,x,y,card.width,card.height) break
        when 'two' then
          @paint_symbol(g,x,y,card.width,card.height/2)
          @paint_symbol(g,x,y+card.height/2,card.width,card.height/2)
          break
        when 'three' then
          @paint_symbol(g,x,y,card.width,card.height/3)
          @paint_symbol(g,x,y+card.height/3,card.width,card.height/3)
          @paint_symbol(g,x,y+card.height*2/3,card.width,card.height/3)
          break

    paint_symbol: (g,x,y,w,h) ->
      striped = @shading.string == 'striped'
      switch(@symbol.string){
        when 'diamond' then  @paint_diamond(g,x,y,w,h) break
        when 'squiggle' then  @paint_squiggle(g,x,y,w,h) break
        when 'oval' then  @paint_oval(g,x,y,w,h) break

    paint_diamond: (g,x,y,w,h) ->
      x=g.canvas.width*x
      y=g.canvas.height*y
      w=g.canvas.width*w
      h=g.canvas.height*h
      g.beginPath()
      g.moveTo(x,h/2+y)
      g.lineTo(w/2+x,y)
      g.lineTo(w+x,h/2+y)
      g.lineTo(w/2+x,h+y)
      g.lineTo(x,h/2+y)
      g.closePath()
      @shade(g)

    paint_oval: (g,x,y,w,h) ->
      w=g.canvas.width*w
      h=g.canvas.height*h
      x=g.canvas.width*x
      y=g.canvas.height*y
      g.beginPath()

      kappa = .5522848
      ox = (w / 2) * kappa // control point offset horizontal
      oy = (h / 2) * kappa // control point offset vertical
      xe = x + w           // x-end
      ye = y + h           // y-end
      xm = x + w / 2       // x-middle
      ym = y + h / 2       // y-middle

      g.beginPath()
      g.moveTo(x, ym)
      g.bezierCurveTo(x, ym - oy, xm - ox, y, xm, y)
      g.bezierCurveTo(xm + ox, y, xe, ym - oy, xe, ym)
      g.bezierCurveTo(xe, ym + oy, xm + ox, ye, xm, ye)
      g.bezierCurveTo(xm - ox, ye, x, ym + oy, x, ym)
      g.closePath()
      @shade(g)

    paint_squiggle: (g,x,y,w,h) ->
      x=g.canvas.width*x
      y=g.canvas.height*y
      w=g.canvas.width*w
      h=g.canvas.height*h

      g.beginPath()
      g.moveTo(w+x,h/2-h/5+y)
      g.bezierCurveTo(w/2+x,-h/2-h/5+y,w/2+x,h*3/2-h/5+y,x,h/2-h/5+y)
      g.lineTo(x,h/2+h/5+y)
      g.bezierCurveTo(w/2+x,h*3/2+h/5+y,w/2+x,-h/2+h/5+y,w+x,h/2+h/5+y)
      g.lineTo(w+x,h/2-h/5+y)
      @shade(g)

    shade: (g) ->
      switch(@shading.string){
        when 'solid' then g.fill() break
        when 'striped' then g.fill() break
        when 'open' then g.stroke() break


    @width:1/3
    @height:1/4
    @increment_offsets: ->
        number.offset++symbol.offset++shading.offset++color.offset++
    @get_third: (a,b) ->
      if a==b
        return a
      if((a==1 || b==1) && (a==2 || b==2))
        return 0
      if((a==2 || b==2) && (a==0 || b==0))
        return 1
      if((a==0 || b==0) && (a==1 || b==1))
        return 2
