using UnityEngine;
using System.Collections;


public enum MorphType
{
	MORPH_SIMPLE = 0,
	MORPH_EASEIN,
	MORPH_EASEOUT,
	MORPH_BUBBLE
};

public class MorphObject{
	public float val=0;
	public float dVal;
	
	protected float morphFrame;
	
	protected float delay;
	
	protected float morphSpeed;//normally = 1
	protected float duration;
	protected float startVal;
	protected float stopVal;
	
	protected float range;
	protected MorphType type;

	public MorphObject() {
		val = 0;
		dVal = 0;
		delay = 0;
		morphFrame = 0;
		duration = 0;
		}

	public void Update(){
		if (delay > 0)
		{
			delay -= 1;
		}
		if (delay <= 0)
		{
			//check frame first
			morphFrame += 1;
			if (morphFrame >= duration)
			{
				morphFrame = duration;
				float oldVal = val;
				val = stopVal;
				dVal = val-oldVal;
			}
			else
			{
				switch (type)
				{
				case MorphType.MORPH_SIMPLE:
				{
					float diff = (stopVal - startVal);
					float dt = morphFrame/duration;
					float oldVal = val;
					val = startVal + diff*dt;
					dVal = val-oldVal;
				}
					break;
				case MorphType.MORPH_EASEIN:
				{
					float diff = (stopVal - startVal);
					float dt = Mathf.Sin(Mathf.PI/2*morphFrame/duration);
					float oldVal = val;
					val = startVal + diff*dt;
					dVal = val-oldVal;
				}
					break;
				case MorphType.MORPH_EASEOUT:
				{
					float diff = (stopVal - startVal);
					float dt = Mathf.Cos(Mathf.PI/2*morphFrame/duration-Mathf.PI/2);
					float oldVal = val;
					val = startVal + diff*dt;
					dVal = val-oldVal;
				}
					break;
				case MorphType.MORPH_BUBBLE:
				{
					float diff = (stopVal - startVal);
					float dt = Mathf.Sin(Mathf.PI/2*morphFrame/duration);
					float rangeT = (Mathf.Sin(Mathf.PI*8*morphFrame/duration)*(1-morphFrame/duration))*range;
					float oldVal = val;
					val = startVal + diff*dt + rangeT;
					dVal = val-oldVal;
				}
					break;
				}
			}
		}
	}

	public bool IsFinish(){
		return (morphFrame>=duration);
	}
	
	public bool IsDelayed(){
		return (delay > 0);
	}

	public void setVal(float _val){
		val = _val;
		stopVal = _val;
		startVal = _val;
	}
	
	public void morphSimple(float _start,float _end,float _dur,float _delay){
		morphSimple(_start, _end, _dur);
		delay = _delay;
	}
	
	public void morphEasein(float _start,float _end,float _dur,float _delay){
		morphEasein(_start, _end, _dur);
		delay = _delay;
	}
	
	public void morphEaseout(float _start,float _end,float _dur,float _delay){
		morphEaseout(_start,_end,_dur);
		delay = _delay;
	}
	
	public void morphBubble(float _start,float _end,float _range,float _dur,float _delay){
		morphBubble(_start, _end, _range, _dur);
		delay = _delay;
	}
	
	public void morphSimple(float _start,float _end,float _dur){
		morphSimple(_end,_dur);
		startVal = _start;
		val = _start;
	}
	
	public void morphSimple(float _end,float _dur){
		dVal = 0;
		delay = 0;
		morphFrame = 0;
		startVal = val;
		stopVal = _end;
		duration = _dur;
		type = MorphType.MORPH_SIMPLE;
	}
	
	public void morphEasein(float _start,float _end,float _dur){
		morphEasein(_end,_dur);
		startVal = _start;
		val = _start;
	}
	
	public void morphEasein(float _end,float _dur){
		dVal = 0;
		delay = 0;
		morphFrame = 0;
		startVal = val;
		stopVal = _end;
		duration = _dur;
		type = MorphType.MORPH_EASEIN;
	}
	
	public void morphEaseout(float _start,float _end,float _dur){
		morphEaseout(_end,_dur);
		startVal = _start;
		val = _start;
	}
	
	public void morphEaseout(float _end,float _dur){
		dVal = 0;
		delay = 0;
		morphFrame = 0;
		startVal = val;
		stopVal = _end;
		duration = _dur;
		type = MorphType.MORPH_EASEOUT;
	}
	
	public void morphBubble(float _start,float _end,float _range,float _dur){
		dVal = 0;
		delay  = 0;
		morphFrame = 0;
		startVal = _start;
		stopVal = _end;
		duration = _dur;
		type = MorphType.MORPH_BUBBLE;
		range = _range;
		val = startVal;
	}

}
