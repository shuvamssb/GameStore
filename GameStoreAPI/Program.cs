using GameStoreAPI.Dtos;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

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

//GET /games
app.MapGet("games/{id}",(int id) => games.Find(game =>game.Id == id))
.WithName("GetGame");

//POST /games
app.MapPost("games",(CreateGameDto newGame) =>
{
    GameDto game= new (
        games.Count+1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute("GetGame",new {id = game.Id},game);
});

app.Run();
