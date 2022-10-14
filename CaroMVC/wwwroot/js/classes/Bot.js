class Bot {
    name;
    moves;
    userMoves;
    game;
    constructor(name, game) {
        this.name = name;
        this.moves = [];
        this.userMoves = [];
        this.game = game;
    }
    getRandomMove(userMove) {
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
        return move;
    }
    isThreat(line) {
        return line.length >= 3;
    }
    getBestMove(userMove) {
        const playerMoves = game.moves.filter(m => m.userName != this.name);
        const allValidHorLines = playerMoves.map(m => game.getHorLine(m)).filter(l => this.isThreat(l));
        const horMoves = allValidHorLines.map(line => {
            const firstMove = line[0];
            const firstRow = firstMove.row;
            const firstCol = firstMove.col;
            if (!game.isMoveExist(firstRow, firstCol - 1))
                return new Move(firstRow, firstCol - 1);

            const lastMove = line[line.length - 1];
            const lastRow = lastMove.row;
            const lastCol = lastMove.col;
            if (!game.isMoveExist(lastRow, lastCol + 1))
                return new Move(lastRow, lastCol + 1);
        }).filter(move => move != null);
        if (horMoves[0] != null) {
            return horMoves[0];
        }
        const allValidVerLines = playerMoves.map(m => game.getVerLine(m)).filter(l => this.isThreat(l));
        const verMoves = allValidVerLines.map(line => {
            const firstMove = line[line.length - 1];
            const firstRow = firstMove.row;
            const firstCol = firstMove.col;
            if (!game.isMoveExist(firstRow - 1, firstCol))
                return new Move(firstRow - 1, firstCol);

            const lastMove = line[0];
            const lastRow = lastMove.row;
            const lastCol = lastMove.col;
            if (!game.isMoveExist(lastRow + 1, lastCol))
                return new Move(lastRow + 1, lastCol);
        }).filter(move => move != null);
        if (verMoves[0] != null) {
            return verMoves[0];
        }
        const allValidLRDLines = playerMoves.map(m => game.getLRDLine(m)).filter(l => this.isThreat(l));
        const lrdMoves = allValidLRDLines.map(line => {
            const firstMove = line[0];
            console.table(firstMove);
            const firstRow = firstMove.row;
            const firstCol = firstMove.col;
            if (!game.isMoveExist(firstRow + 1, firstCol - 1))
                return new Move(firstRow + 1, firstCol - 1);

            const lastMove = line[line.length - 1];
            const lastRow = lastMove.row;
            const lastCol = lastMove.col;
            if (!game.isMoveExist(lastRow - 1, lastCol + 1))
                return new Move(lastRow - 1, lastCol + 1);
        }).filter(move => move != null);
        if (lrdMoves[0] != null) {
            return lrdMoves[0];
        }
        const allValidRLDLines = playerMoves.map(m => game.getRLDLine(m)).filter(l => this.isThreat(l));
        const rldMoves = allValidRLDLines.map(line => {
            const firstMove = line[0];
            console.table(firstMove);
            const firstRow = firstMove.row;
            const firstCol = firstMove.col;
            if (!game.isMoveExist(firstRow - 1, firstCol - 1))
                return new Move(firstRow - 1, firstCol - 1);

            const lastMove = line[line.length - 1];
            const lastRow = lastMove.row;
            const lastCol = lastMove.col;
            if (!game.isMoveExist(lastRow + 1, lastCol + 1))
                return new Move(lastRow + 1, lastCol + 1);
        }).filter(move => move != null);
        if (rldMoves[0] != null) {
            return rldMoves[0];
        }
        const move = this.getRandomMove(userMove);
        return move;
    }
    createMove(userMove) {
        this.userMoves.push(userMove);
        const move = this.getBestMove(userMove);
        this.moves.push(move);
        return move;
    }

}