class Bot {
    name;
    game;
    constructor(name, game) {
        this.name = name
        this.game = game
    }
    createMove(userMove) {
        let row = 0;
        let col = 0;
        do {
            row = Math.floor(Math.random() * (game.row - 1));
            col = Math.floor(Math.random() * (game.col - 1));
        }
        while (game.isMoveExist(row, col));
        return new Move(row, col, bot.name);
    }

}