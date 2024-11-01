using GameStore.Dtos;

namespace GameStore.Endpoints
{
    public static class GamesEndpoints
    {
         const string GetGameEndpointName = "GetGame";
        private static readonly List<GameDto> games = [
        new (
        1,
        "Street Fighter 2",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)
    ),
    new (
        2,
        "Street Fighter 2",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)
    ),
    new (
        3,
        "Street Fighter 2",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)
    ),
    new (
        4,
        "Street Fighter 2",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)
    )
    ];
        public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
        {
            var group =app.MapGroup("games").WithParameterValidation();
            // Get/games
            group.MapGet("/", () => games);

            //Gat /games/1
            group.MapGet("/{id}", (int id) =>
            {
                GameDto? game = games.Find(game => game.Id == id);
                return game is null ? Results.NotFound() : Results.Ok(game);

            }

            ).WithName(GetGameEndpointName);
            //Post /games

            group.MapPost("/", (CreateGameDto newGame) =>
            {
                GameDto game = new(
                    games.Count + 1,
                    newGame.Name,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate
            );
                games.Add(game);
                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
            });

            //PUt
            group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
            {
                var index = games.FindIndex(game => game.Id == id);
                if (index == -1)
                {
                    return Results.NotFound();
                }
                games[index] = new GameDto(
                    id,
                    updatedGame.Name,
                    updatedGame.Genre,
                    updatedGame.Price,
                    updatedGame.ReleaseDate
                );
                return Results.NoContent();
            });

            //DELETE

            group.MapDelete("/{id}", (int id) =>
            {
                games.RemoveAll(game => game.Id == id);

                return Results.NoContent();
            });
            return group;
        }
    }
}