using GameStoreAPI.Dtos;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

const string GetGameEndpointName= "GetGame";

List<GameDto> games =[
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
        new(1,
        "FIFA 23",
        "Sports",
        55.99M,
        new DateOnly(2023,4,23))
];


//GET /games
app.MapGet("games",() =>games);

//GET /games/1
app.MapGet("games/{id}",(int id) => games.Find(game =>game.Id == id))
.WithName(GetGameEndpointName); // withname is used to defined an endpoint

//POST /games
app.MapPost("games",(CreateGameDto newGame) =>
{
    //Add game to GameDto so game variable has been initialised
    GameDto game= new (
        games.Count+1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    //Return whether the game data has been created or not
    return Results.CreatedAtRoute(GetGameEndpointName,new {id = game.Id},game);
    
});

app.Run();
