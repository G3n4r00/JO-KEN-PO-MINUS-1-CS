// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
Console.OutputEncoding = System.Text.Encoding.UTF8;


Random random = new Random();

string arquivoPontuacoes = "pontuacoes.json";

// Função para salvar as pontuações dos jogadores em um arquivo JSON
void SalvarPontuacoes(Dictionary<string, int> jogadores)
{
    try
    {
        
        string json = JsonSerializer.Serialize(jogadores, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(arquivoPontuacoes, json);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao salvar o arquivo: {ex.Message}");
    }
}

// Função para carregar as pontuações dos jogadores a partir de um arquivo JSON
Dictionary<string, int> CarregarPontuacoes()
{
    // Verifica se o arquivo de pontuações existe
    if (File.Exists(arquivoPontuacoes))
    {
        // Lê o conteúdo do arquivo JSON
        string json = File.ReadAllText(arquivoPontuacoes);

        // Desserializa o conteúdo JSON para um dicionário de jogadores e suas pontuações
        // Se a desserialização falhar (retornar nulo), cria um novo dicionário vazio
        return JsonSerializer.Deserialize<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
    }

    return new Dictionary<string, int>();
}

//Função para decidir a jogada final do computador com base nas jogadas do jogador
int jogadaComputador(int j1, int j2, int c1, int c2)
{
    var dicJogadasVencidas = new Dictionary<int, int>
    {
        { 0, 2 }, // Pedra vence Tesoura
        { 1, 0 }, // Papel vence Pedra
        { 2, 1 }  // Tesoura vence Papel
    };

    int avaliarJogada(int jogada)
    {
        int score = 0;

        if (j1 == dicJogadasVencidas[jogada] || j2 == dicJogadasVencidas[jogada])
            score += 2; // Se a jogada ganha contra alguma carta do jogador, melhor para o computador

        if (j1 == jogada || j2 == jogada)
            score += 1; // Se empata com alguma carta do jogador, menor impacto 

        return score;
    }

    int scoreC1 = avaliarJogada(c1);
    int scoreC2 = avaliarJogada(c2);

    return scoreC1 > scoreC2 ? c1 : c2; // Maior score significa maior chance de ganhar
}

// Função que decide o ganhador ao final de cada jogo
bool jogadaFinal(int usr, int comp)
{
    // true: usuario ganha
    // false: computador ganha

    var vencedoras = new Dictionary<int, int>
    {
        { 0, 2 },
        { 1, 0 },
        { 2, 1 },
    };

    if (comp == vencedoras[usr])
    {
        return true;
    }
    else
    {
        return false;
    }
}

//Função que valida a entrada do usuário
static int Valido(string input)
{
    // Try to parse the input as an integer
    if(int.TryParse(input, out _))
    {
        int number = Convert.ToInt32(input);
        if(number <= 2 && number >= 0)
        {
            return number;
        }
        else
        {
            Console.WriteLine("Opcao Invalida: Jogue com o que tem...!");
            return -1;
        }
    }
    else
    {
        Console.WriteLine("Opcao Invalida: Isso nem é número inteiro!");
        return -1;
    }
}


Dictionary<string, int> jogadores = CarregarPontuacoes();
if (!jogadores.ContainsKey("com"))
{
    jogadores["com"] = 0;
}

void addJogador(String nome, Dictionary<String, int> jogadoresFuncao)
{
    if (!jogadoresFuncao.ContainsKey(nome)){
        jogadoresFuncao[nome] = 0;
    }
}

var Elementos = new Dictionary<int, string>
    {
        { 0, "pedra" },
        { 1, "papel" },
        { 2, "tesoura" },
    };



Console.WriteLine("===========================================");
Console.WriteLine("         BEM-VINDO AO JO KEN PO -1        ");
Console.WriteLine("===========================================");
Console.WriteLine();
Console.WriteLine("Regras do jogo:");
Console.WriteLine("1. Você receberá três opções: Pedra, Papel e Tesoura.");
Console.WriteLine("2. Antes de cada rodada, você deve escolher duas opções.");
Console.WriteLine("3. Com suas duas opções prontas, você e seu adversário devem escolher apenas uma de suas 2 opções.");
Console.WriteLine("4. O resultado segue as regras do Pedra, Papel e Tesoura tradicional:");
Console.WriteLine("   - Pedra vence Tesoura");
Console.WriteLine("   - Tesoura vence Papel");
Console.WriteLine("   - Papel vence Pedra");
Console.WriteLine("5. Se ambos escolherem a mesma opção, o jogo empata.");
Console.WriteLine();
Console.WriteLine("Use sua estratégia para prever a jogada do oponente!");
Console.WriteLine();
Console.WriteLine("Vamos jogar?");
Console.WriteLine("1 - Sim ou 0 - Não");
Console.WriteLine();





while (true)
{
    if (Console.ReadKey().KeyChar == '1')

    {
        Console.WriteLine("\n===========================================");
        Console.WriteLine("             PREPARE-SE PARA JOGAR         ");
        Console.WriteLine("===========================================");
        Console.WriteLine();

        Console.WriteLine("\nEscreva seu nome de usuário...");
        String nomeJogador = Console.ReadLine();
        addJogador(nomeJogador, jogadores);

        while (true)
        {
            string jogadorSelec1 = " ";
            string jogadorSelec2 = " ";
            int mao1 = -1;
            int mao2 = -1;

            while (mao1 == -1)
            {
                Console.WriteLine("\nEscolha sua primeira opção: 0 - Pedra ✊, 1 - Papel ✋ ou 2 - Tesoura ✌");
                jogadorSelec1 = Console.ReadLine();
                mao1 = Valido(jogadorSelec1);
                
            }


            while (mao2 == -1)
            {
                Console.WriteLine("\nEscolha sua segunda opção: 0 - Pedra ✊, 1 - Papel ✋ ou 2 - Tesoura ✌");
                jogadorSelec2 = Console.ReadLine();
                mao2 = Valido(jogadorSelec2);
               
            }


            // Computador selecionando suas jogadas
            List<int> opcoes = new List<int> { 0, 1, 2 };
            opcoes = opcoes.OrderBy(x => random.Next()).ToList();

            int compMao1 = opcoes[0];
            int compMao2 = opcoes[1];

            Console.WriteLine($"Você jogou {Elementos[mao1]} e {Elementos[mao2]}");
            Console.WriteLine($"O computador jogou {Elementos[compMao1]} e {Elementos[compMao2]}");
            Console.WriteLine();

            Console.WriteLine("Agora vem a parte interessante...");
            int valido = -1;
            string finalJogador = " ";
            int finalJogadorNum = 0;

            while (valido == -1)
            {
                Console.WriteLine($"\nSelecione sua jogada final: 1 - {Elementos[mao1]} ou 2 - {Elementos[mao2]}");
                finalJogador = Console.ReadLine();
                if(finalJogador == "1")
                {
                    finalJogadorNum = mao1;
                    valido = 0;
                }
                else if(finalJogador == "2")
                {
                    finalJogadorNum = mao2;
                    valido = 0;
                }
                else
                {
                    Console.WriteLine("Selecione uma de suas mãos...");
                }

                
            }

            int finalComp = jogadaComputador(mao1, mao2, compMao1, compMao2);




            Console.WriteLine("Jogadas Interessantes....");
            Console.WriteLine($"\n{nomeJogador} jogou {Elementos[finalJogadorNum]}... O computador jogou {Elementos[finalComp]}");
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(". ");
                Thread.Sleep(500);
            }

            if (finalJogadorNum == finalComp || finalJogadorNum == finalComp)
            {
                Console.WriteLine("\nÉ um EMPATEEEE");
            }
            else if (jogadaFinal(finalJogadorNum, finalComp))
            {
                Console.WriteLine("\nVOCE GANHOU!!!");
                jogadores[nomeJogador] += 1;
            }
            else
            {
                Console.WriteLine($"\nDesculpa! Não foi dessa vez... 😕 ");
                jogadores["com"] += 1;
            }

            Console.WriteLine("\nDeseja Contiuar Jogando?");
            Console.WriteLine("\n1 - Sim ou 0 - Não");
            if (Console.ReadKey().KeyChar == '0')
            {
                break;
            }
            
        }

        SalvarPontuacoes(jogadores);
        break;


    }
    else if (Console.ReadKey().KeyChar == '0')
    {
        Console.WriteLine("👋 Tchau! Até a próxima");
        break;
    }
    else
    {
        Console.WriteLine("Entrada Invalida.");
    }
}

    

// Criando o ranking
var ranking = jogadores
    .OrderByDescending(u => u.Value) // Ordena por valor decrescente
    .Select((user, index) => new { Nome = user.Key, Pontos = user.Value, Index = index + 1 })
    .ToList();

int posicao = 1;
Console.WriteLine("\n🏆 Ranking Atual 🏆");
for (int i = 0; i < ranking.Count; i++)
{
    if (i > 0 && ranking[i].Pontos < ranking[i - 1].Pontos)
    {
        posicao = i + 1; // Atualiza a posição apenas se o valor for menor que o anterior
    }

    Console.WriteLine($"{posicao}º lugar: {ranking[i].Nome} ({ranking[i].Pontos} pontos)");
}

Console.WriteLine("👋 Tchau! Até a próxima");


//int jogadaComputador(int j1, int j2, int c1, int c2)
//{
//    int jogada1 = 0;
//    int jogada2 = 0;

//    var dicJogadasVencidas = new Dictionary<int, int>
//    {
//        { 0, 2 },
//        { 1, 0 },
//        { 2, 1 },
//    };

//    if (j1 == dicJogadasVencidas[c1])
//    {
//        jogada1 += 2;
//    }
//    else if (j1 == c1)
//    {
//        jogada1 += 1;
//    }

//    if (j2 == dicJogadasVencidas[c1])
//    {
//        jogada1 += 2;
//    }
//    else if (j2 == c1)
//    {
//        jogada1 += 1;
//    }



//    if (j1 == dicJogadasVencidas[c2])
//    {
//        jogada2 += 2;
//    }
//    else if (j1 == c2)
//    {
//        jogada2 += 1;
//    }

//    if (j2 == dicJogadasVencidas[c2])
//    {
//        jogada2 += 2;
//    }
//    else if (j2 == c2)
//    {
//        jogada2 += 1;
//    }

//    if (jogada1 > jogada2)
//    {
//        return c1;
//    }
//    else
//    {
//        return c2;
//    }
//}
