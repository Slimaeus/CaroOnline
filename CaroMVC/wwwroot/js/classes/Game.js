class Game {
    #moves;

    constructor() {
        this.#moves = []
    }

    // Check if a user win
    isWinner(move) {
        return true
    }

    get moves() {
        return this.#moves
    }

    set moves(value) {
        this.#moves = value;
    }
    getMove(row, col) {
        return this.#moves.find(move => move.row === row &&  move.col === col)
    }
    // Check if a move exists in game
    isMoveExists(row, col) {
        return this.#moves.any(m => m.row === row && m.col === col)
    }
    // Get Horizontal Line
    getHorLine(move) {
        const line = [move]
        // Get on the Left
        let tempMove = move.clone()
        while (this.isMoveExists(tempMove.row, tempMove.getLeftCol())) {
            
        }
        move.getLeftCol()
        move.getRightCol()
        return line
    }
    // Get Vertical Line
    getVerLine() {}
    // Get Left to Right Diagonal Line
    getLRDLine() {}
    // Get Right to Left Diagonal Line
    getRLDLine() {}
    // Count move number in a line
    lineCount(line) {
        
    }
}