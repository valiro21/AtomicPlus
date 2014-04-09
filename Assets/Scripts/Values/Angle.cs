using UnityEngine;
using System.Collections;

public class Angle {
	float degree;
	float radian;
	static Angle FULL = new Angle(360f, false);

	void RepairAngle ( ) {
		long ratio = (long)(degree / 360f);
		degree -= ratio * 360f;
		
		if (degree < 0f)
			degree += 360f;
		if (degree > 360f)
			degree -= 360f;
		
		radian = degree * Mathf.PI / 180f;
	}

	Angle () {
		degree = 0f;
		radian = 0f;
	}

	Angle (float a) {
		degree = a;
		RepairAngle ();
	}

	Angle (float a, bool f) {
		degree = a;
		if ( f )
			RepairAngle ();
		else
			radian = (a * Mathf.PI) / 180f;
	}

	public float Sin () {
		return Mathf.Sin (radian);
	}

	public static Angle HalfWay ( Angle x, Angle y ) {
		Angle k = new Angle ();
		if ( x <= y )
			k = (x + y ) / 2f;
		else
			k = ( x + y + 360f ) / 2f;
		return k;
	}

	public bool IsBetween ( Angle x, Angle y ) {
		if (y < x) {
			if ( this < y || this > x )
				return true;
			return false;
		}
		else
			if ( x < this && this < y )
				return true;

		return false;
	}

	public Angle ArcLengthTo ( Angle end ) {
		if (degree < end.degree)
			return end - this;
		else
			return FULL - this + end;
	}

	public Vector3 PointByRadius (float Radius ) {
		float x = ((long)((GameController.Ring.x - Radius * Mathf.Cos (radian)) * 10000f)) / 10000f;
		float y = ((long)((GameController.Ring.y + Radius * Mathf.Sin (radian)) * 10000f)) / 10000f;
		return new Vector3 (x, y, GameController.Ring.z);
	}

	public static Vector3 PointByRadius (Angle k, float Radius ) {
		return k.PointByRadius ( Radius );
	}

	public static implicit operator Angle(float a) {
		Angle k = new Angle(a);
		return k;
	}

	public static implicit operator float(Angle a) {
		return a.degree;
	}

	public static Angle operator +(Angle a, Angle b) {
		return a.degree + b.degree;
	}

	public static Angle operator -(Angle a, Angle b) {
		return a.degree - b.degree;
	}


	public static Angle operator *(Angle a, Angle b) {
		return a.degree * b.degree;
	}

	public static Angle operator /(Angle a, Angle b) {
		return a.degree / b.degree;
	}


	public static bool operator <(Angle a, Angle b) {
		return a.degree < b.degree;
	}
	
	public static bool operator >(Angle a, Angle b) {
		return a.degree > b.degree;
	}

	public static bool operator ==(Angle a, Angle b) {
		return a.degree == b.degree;
	}

	public static bool operator !=(Angle a, Angle b) {
		return a.degree != b.degree;
	}

	public bool Equals ( Angle a ) {
		return degree == a.degree;
	}

	public static bool operator <=(Angle a, Angle b) {
		return a.degree <= b.degree;
	}
	
	public static bool operator >=(Angle a, Angle b) {
		return a.degree >= b.degree;
	}
}
