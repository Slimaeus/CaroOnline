class Move {
    row;
    col;
    userName;
    color;
    constructor(row, col, userName) {
        this.row = Number(row)
        this.col = Number(col)
        this.userName = userName
    }
    
    isSameLine(move) {
        return this.userName === move.userName;
    }
}