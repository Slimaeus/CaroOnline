class Bot {
    name;
    moves;
    userMoves;
    game;
    constructor(name, game) {
        this.name = name
        this.moves = []
        this.userMoves = [];
        this.game = game
    }
    createMove(userMove) {
        this.userMoves.push(userMove);
        let row = userMove.row;
        let col = userMove.col;
        const MIN = -1;
        const MAX = 1;
        do {
            row = row + Math.floor(Math.random() * (MAX - MIN + 1) + MIN);
            col = col + Math.floor(Math.random() * (MAX - MIN + 1) + MIN);
        }
        while (game.isMoveExist(row, col));
        const move = new Move(row, col, bot.name);
        this.moves.push(move);
        return move;
    }

}