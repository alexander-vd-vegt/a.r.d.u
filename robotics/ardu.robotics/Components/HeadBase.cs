namespace ardu.robotics.components;

public abstract class HeadBase
{
    public HeadBase(){
        _turnLimit = 180;
        _tiltLimit = 45;
    }
    protected int _turnPosition;
    protected int _tiltPosition;

    protected int _turnLimit;
    protected int _tiltLimit;

    public void Turn(int position){
        if(position >= 0 &&  position <= _turnLimit){
            this.OnTurn(position);
        }
        else{
            throw new InvalidOperationException($"unable to perform turn, given position must be between 0 and {_turnLimit}");
        }

    }

    public void Tilt(int position){
        if(position >= 0 &&  position <= _tiltLimit){
            this.OnTurn(position);
        }
        else{
            throw new InvalidOperationException($"unable to perform tilt, given position must be between 0 and {_tiltLimit}");
        }
    }

    public abstract void OnTurn(int position);
    public abstract void OnTilt(int position);
}
