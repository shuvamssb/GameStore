using GameStoreAPI.Dtos;

namespace GameStoreAPI.Endpoints;

public static class GameEndpoints
{

    const string GetGameEndpointName = "GetGame";

    //modifier used is private
    private static readonly List<GameDto> games = [    
    new(1,
            "Genshin Impact",
            "RPG",
            109.99M,
            new DateOnly(2020,10,28)),
    new(2,
            "RDR2",
            "Action",
            1500.01M,
            new DateOnly(2017,2,28)),
    new(3,
            "FIFA 23",
            "Sports",
            55.99M,
            new DateOnly(2023,4,23))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
            var group = app.MapGroup("games");

            //GET /games
            group.MapGet("/", () => games);

            //GET /games/1
            group.MapGet("/{id}", (int id) =>
            {
                GameDto?  game=games.Find(game => game.Id == id);

                return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GetGameEndpointName); // withname is used to defined an endpoint

            //POST /games
            group.MapPost("/", (CreateGameDto newGame) =>
            {
                //Add game to GameDto so game variable has been initialised
                GameDto game = new(
                    games.Count + 1,
                    newGame.Name,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate
                );

                games.Add(game);

                //Return whether the game data has been created or not
                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);

            });

            //PUT /Games
            group.MapPut("{id}", (int id, UpdateGameDTO updatedGame) =>
            {
                //find existing game and then replace the game with the new
                //finding the index of the game
                var index = games.FindIndex(game => game.Id == id);

                if(index == -1)
                {
                    return Results.NotFound();
                }

                //updating the game]
                games[index] = new GameDto(
                    id,
                    updatedGame.Name,
                    updatedGame.Genre,
                    updatedGame.Price,
                    updatedGame.ReleaseDate
                );

                return Results.NoContent();//for output operation
            });


            //DELETE /games/1
            group.MapDelete("/{id}", (int id) => 
            {
                games.RemoveAll(game=>game.Id == id);

                
            return Results.NoContent();//for output operation
            });

            return group;
    }
}
