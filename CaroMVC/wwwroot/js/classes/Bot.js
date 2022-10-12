class Bot {
    name;
    game;
    constructor(name, game) {
        this.name = name
        this.game = game
    }
    createMove(userMove) {
        const row = userMove.row + 1;
        const col = userMove.col + 1;
        return new Move(row, col, bot.name);
    }

}