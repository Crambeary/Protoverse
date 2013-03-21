/*
//=====================================
//simple physics functions

//(px,py) is projection vector, (dx,dy) is ssurface normal, obj is 
//other object.
function ReportCollisionVsWorld(px,py,dx,dy,obj)
{

	var p = this.pos;
	var o = this.oldpos;
	
	//calc velocity
	var vx = p.x - o.x;
	var vy = p.y - o.y;
	
	//find component of velocity parallel to collision normal
	var dp = (vx*dx + vy*dy);
	var nx = dp*dx;//project velocity onto collision normal
	
	var ny = dp*dy;//nx,ny is normal velocity
	
	var tx = vx-nx;//px,py is tangent velocity
	var ty = vy-ny;

	//we only want to apply collision response forces if the object is travelling into, and not out of, the collision
	var b,bx,by,f,fx,fy;
	if(dp < 0)
	{
		f = FRICTION;
		fx = tx*f;
		fy = ty*f;		
		
		b = 1+BOUNCE;//this bounce constant should be elsewhere, i.e inside the object/tile/etc..
		
		bx = (nx*b);
		by = (ny*b);
	
	}
	else
	{
		//moving out of collision, do not apply forces
		bx = by = fx = fy = 0;

	}


	p.x += px;//project object out of collision
	p.y += py;
	
	o.x += px + bx + fx;//apply bounce+friction impulses which alter velocity
	o.y += py + by + fy;

}

function IntegrateVerlet()
{
	var d = DRAG;
	var g = GRAV;
	
	p = this.pos;
	o = this.oldpos;
		
	ox = o.x; //we can't swap buffers since mcs/sticks point directly to vector2s..
	oy = o.y;
	o.x = px = p.x;		//get vector values
	o.y = py = p.y;		//p = position  
						//o = oldposition
			
	//integrate	
	p.x += (d*px) - (d*ox);
	p.y += (d*py) - (d*oy) + g;	
}

function CollideCircleVsWorldBounds()
{
	var p = this.pos;
	var r = this.r;
	
	//collide vs. x-bounds
	//test XMIN
	var dx = XMIN - (p.x - r);
	if(0 < dx)
	{
		//object is colliding with XMIN
		this.ReportCollisionVsWorld(dx,0,1,0,null);
	}
	else
	{
		//test XMAX
		dx = (p.x + r) - XMAX;
		if(0 < dx)
		{
			//object is colliding with XMAX
			this.ReportCollisionVsWorld(-dx,0,-1,0,null);
		}
	}
	
	//collide vs. y-bounds
	//test YMIN
	var dy = YMIN - (p.y - r);
	if(0 < dy)
	{
		//object is colliding with YMIN
		this.ReportCollisionVsWorld(0,dy,0,1,null);
	}
	else
	{
		//test YMAX
		dy = (p.y + r) - YMAX;
		if(0 < dy)
		{
			//object is colliding with YMAX
			this.ReportCollisionVsWorld(0,-dy,0,-1,null);
		}
	}	
}
function CollideAABBVsWorldBounds()
{
	var p = this.pos;
	var xw = this.xw;
	var yw = this.yw;
	
	//collide vs. x-bounds
	//test XMIN
	var dx = XMIN - (p.x - xw);
	if(0 < dx)
	{
		//object is colliding with XMIN
		this.ReportCollisionVsWorld(dx,0,1,0,null);
	}
	else
	{
		//test XMAX
		dx = (p.x + xw) - XMAX;
		if(0 < dx)
		{
			//object is colliding with XMAX
			this.ReportCollisionVsWorld(-dx,0,-1,0,null);
		}
	}
	
	//collide vs. y-bounds
	//test YMIN
	var dy = YMIN - (p.y - yw);
	if(0 < dy)
	{
		//object is colliding with YMIN
		this.ReportCollisionVsWorld(0,dy,0,1,null);
	}
	else
	{
		//test YMAX
		dy = (p.y + yw) - YMAX;
		if(0 < dy)
		{
			//object is colliding with YMAX
			this.ReportCollisionVsWorld(0,-dy,0,-1,null);
		}
	}	
}*/