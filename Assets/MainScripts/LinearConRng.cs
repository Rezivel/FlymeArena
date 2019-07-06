using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LinearConRng {
	private const long a = 25214903917;
  	private const long c = 11;
  	public long seed;
  	public LinearConRng(long seed){
		this.seed = seed;
	}
	private int next(int bits){
    	seed = (seed * a + c) & ((1L << 48) - 1);
    	return (int)(seed >> (48 - bits));
	}
	public double NextDouble(){
		return (((long)next(26) << 27) + next(27)) / (double)(1L << 53);
	}
	public int NextInt(int lo,int hi){
		return (int)((hi - lo) * NextDouble() + lo);
	}
}
