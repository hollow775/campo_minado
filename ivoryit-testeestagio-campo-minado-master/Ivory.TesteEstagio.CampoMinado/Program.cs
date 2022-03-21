using System;

namespace Ivory.TesteEstagio.CampoMinado
{
    class Program
    {
        static void Main(string[] args)
        {
            var campoMinado = new CampoMinado();
            Console.WriteLine("Início do jogo\n=========");
            Console.WriteLine(campoMinado.Tabuleiro);

            // Realize sua codificação a partir deste ponto, boa sorte!

            //matriz que irá amarzenar a posicao das bombas/ colocar bandeiras
            char[,] matrizBombas = new char[9,9]{
                { '-', '-', '-', '-', '-', '-', '-', '-', '-' },
                { '-', '-', '-', '-', '-', '-', '-', '-', '-' },
                { '-', '-', '-', '-', '-', '-', '-', '-', '-' },
                { '-', '-', '-', '-', '-', '-', '-', '-', '-' },
                { '-', '-', '-', '-', '-', '-', '-', '-', '-' },
                { '-', '-', '-', '-', '-', '-', '-', '-', '-' },
                { '-', '-', '-', '-', '-', '-', '-', '-', '-' },
                { '-', '-', '-', '-', '-', '-', '-', '-', '-' },
                { '-', '-', '-', '-', '-', '-', '-', '-', '-' }
            };
            
            //primeiramente crio uma matriz contendo o tabuleiro para fazer as comparações
            var campo = "";
            char[,] matrizCampo = new char[9,9];
            var contador = 0;

            //enquanto não ganharmos vamos ter que ficar jogando.
            while (campoMinado.JogoStatus == 0)
            {
                //esse passo é necessário para sempre atualizar o tabuleiro
                campo = campoMinado.Tabuleiro.Replace("\n","");
                campo = campo.Replace("\r", "");
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        matrizCampo[i, j] = campo[contador];
                        contador++;
                    }
                }
                contador = 0;
                //for para percorrer a matriz e posicionar as bandeiras
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        matrizBombas = PosicionarBandeira(matrizCampo, matrizBombas, i, j);
                    }
                }
                
                //for para percorrer a matriz e abrir as celulas que temos certeza que nao contem minas.
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        AbrirCelulas(campoMinado, matrizCampo, matrizBombas, i,j);
                    }
                }
                Console.WriteLine($"{campoMinado.Tabuleiro}\n ------ \n");
            
                if (campoMinado.JogoStatus == 1)
                {
                    Console.WriteLine("\r\r GANHOU!!");
                }
                if(campoMinado.JogoStatus == 2)
                {
                    Console.WriteLine("\r\r PERDEU!!");
                }
            }
        }

        //Funcao que posiciona as bandeiras, isto é, onde temos bombas.
        public static char[,] PosicionarBandeira(char[,] campo, char[,] bombas, int linha, int coluna)
        {
            //Devemos olhar nos oito quadrados ao redor de onde vamos abrir para saber se é seguro ou não...
            //temos a seguinte disposição
            // campo[linha-1,coluna-1] , campo[linha-1,coluna] ,campo[linha-1,coluna+1]
            // campo[linha,coluna-1] , célula a verificar!! ,campo[linha,coluna+1]
            // campo[linha+1,coluna-1] , campo[linha+1,coluna] ,campo[linha+1,coluna+1]
            var quantidadeCasasVazias = 0;
            var valorCelula = (int)Char.GetNumericValue(campo[linha, coluna]);
            
            if(campo[linha,coluna] != '0' && campo[linha,coluna] != '-'){
                for (int l = linha - 1; l < linha + 2; l++)
                {
                    for (int c = coluna - 1; c < coluna + 2; c++)
                    {
                        //verificação para caso estivermos em uma célula no canto do campo. e caso tenha uma casa vazia ao lado da célula.
                        if (l >= 0 && l < 9 && c >= 0 && c < 9)
                        {
                            if (campo[l, c] == '-')
                            {
                                quantidadeCasasVazias++;
                            }
                        }
                    }
                }
                if(valorCelula == quantidadeCasasVazias){
                    for (int l = linha - 1; l < linha + 2; l++)
                    {
                        for (int c = coluna - 1; c < coluna + 2; c++)
                        {
                            //verificação para caso estivermos em uma célula no canto do campo.
                            if (l >= 0 && l < 9 && c >= 0 && c < 9)
                            {
                                if(campo[l, c] == '-'){
                                    bombas[l, c] = 'B';
                                }
                            }
                        }
                    }
                }
            }

            
            
            return bombas;
        }
        
        //Funcao para abrir as ce
        public static void AbrirCelulas(CampoMinado campoMinado,char[,] campo, char[,] bombas, int linha, int coluna)
        {
            var quantidadeMinas = 0;
            var valorCelula = (int)Char.GetNumericValue(campo[linha, coluna]);

            //para abrir as celulas precisamos primeiro contar a quantidade de bombas ao redor da celula
            //pegamos somente celulas que contem bombas ao redor
            if (campo[linha, coluna] != '0' && campo[linha, coluna] != '-')
            {
                for (int l = linha - 1; l < linha + 2; l++)
                {
                    for (int c = coluna - 1; c < coluna + 2; c++)
                    {
                        //verificação para caso estivermos em uma célula no canto do campo.
                        if (l >= 0 && l < 9 && c >= 0 && c < 9)
                        {
                            if (bombas[l, c] == 'B')
                            {
                                quantidadeMinas++;
                            }
                        }
                    }
                }
                // após fazer a contagem percorremos novamente as células ao redor da atual e abrimos os espaços que não contem bandeira, ou seja, que não tem mina
                if (valorCelula == quantidadeMinas)
                {
                    for (int l = linha - 1; l < linha + 2; l++)
                    {
                        for (int c = coluna - 1; c < coluna + 2; c++)
                        {
                            //verificação para caso estivermos em uma célula no canto do campo.
                            if (l >= 0 && l < 9 && c >= 0 && c < 9 )
                            {
                                if(campo[l,c] == '-' && bombas[l, c] != 'B'){
                                    campoMinado.Abrir(l+1, c+1);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
