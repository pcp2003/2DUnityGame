using UnityEngine.Rendering;

public class Cell
{   
    public bool isWater;

    public bool isOccupied;

    public Cell(bool isWater, bool isOccupied) {
        this.isWater = isWater;
        this.isOccupied = isOccupied;
    }

    public void setIsOccupied(bool b){
        this.isOccupied = b;
    }
}
