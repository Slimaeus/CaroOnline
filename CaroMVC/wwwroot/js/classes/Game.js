class Game {
    #moves;
    startedDate;

    constructor() {
        this.#moves = []
        this.startedDate = Date.now();
    }
    // Check if a user win
    isWinner(move) {
        const hasValidHorLine = this.getHorLine(move).length >= 5
        const hasValidVerLine = this.getVerLine(move).length >= 5
        const hasValidLRDLine = this.getLRDLine(move).length >= 5
        const hasValidRLDLine = this.getRLDLine(move).length >= 5
        return hasValidHorLine || hasValidVerLine || hasValidLRDLine || hasValidRLDLine
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
    isMoveExist(row, col) {
        return this.#moves.some(m => m.row === row && m.col === col)
    }
    // Get line with gap
    getLine(move, rowGap, colGap) {
        const line = []
        let currMove = move
        while (this.isMoveExist(currMove.row + rowGap, currMove.col + colGap)) {
            const nextMove = this.getMove(currMove.row + rowGap, currMove.col + colGap);
            if (currMove.isSameLine(nextMove))
                line.push(nextMove)
            currMove = nextMove
        }
        return line
    }
    // Get On the Left Line
    getLeftLine(move) {
        return this.getLine(move, 0, -1)
    }
    // Get On the Right Line
    getRightLine(move) {
        return this.getLine(move, 0, 1)
    }
    // Get the Up Line
    getUpLine(move) {
        return this.getLine(move, 1, 0)
    }
    // Get the Down Line
    getDownLine(move) {
        return this.getLine(move, -1, 0)
    }
    // Get the Up Left Line
    getUpLeftLine(move) {
        return this.getLine(move, 1, -1)
    }
    // Get the Up Right Line
    getUpRightLine(move) {
        return this.getLine(move, 1, 1)
    }
    // Get the Down Left Line
    getDownLeftLine(move) {
        return this.getLine(move, -1, -1)
    }
    // Get the Down Line
    getDownRightLine(move) {
        return this.getLine(move, -1, 1)
    }
    // Get Horizontal Line
    getHorLine(move) {
        return [...this.getLeftLine(move), move, ...this.getRightLine(move)]
    }
    // Get Vertical Line
    getVerLine(move) {
        return [...this.getUpLine(move), move, ...this.getDownLine(move)]
    }
    // Get Left to Right Diagonal Line
    getLRDLine(move) {
        return [...this.getUpLeftLine(move), move, ...this.getDownRightLine(move)]
    }
    // Get Right to Left Diagonal Line
    getRLDLine(move) {
        return [...this.getDownLeftLine(move), move, ...this.getUpRightLine(move)]
    }
}