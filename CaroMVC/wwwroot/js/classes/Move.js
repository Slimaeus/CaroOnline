class Move {
    row;
    col;
    userName;

    constructor(row, col, userName) {
        this.row = Number(row)
        this.col = Number(col)
        this.userName = userName
    }
    
    isSameLine(move) {
        return this.userName === move.userName;
    }
    getUpRow() {
        return this.row - 1 
    }
    getDownRow() {
        return this.row + 1
    }
    getLeftCol() {
        return this.col - 1
    }
    getRightCol() {
        return this.col + 1
    }
}