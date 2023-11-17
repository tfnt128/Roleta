# Projeto de Roleta Simples

## Descrição
Este projeto implementa uma roleta simples com 4 cores e 3 telas distintas. A interface do usuário (UI) consiste em uma tela inicial com o título "Roleta", a logo da empresa e um botão "Começar". Quando o usuário clica no único botão, a logo e o botão se movem para a esquerda, revelando a roleta com o botão de girar.

Na segunda tela, ao clicar no botão de girar, a roleta gira e uma mensagem do tipo de 4 mensagens pré-programadas é exibida. Quando a roleta para de girar, a mensagem desaparece, e o aplicativo avança para a última tela.

Na terceira tela, a logo da empresa aparece com a cor correspondente à cor que caiu na roleta, juntamente com uma frase abaixo associada à cor. Após isso, uma seta é exibida para reiniciar a fase.

## Lógica do Código

### Design Pattern Observer
Foi implementado um design pattern Observer utilizando uma interface `IObserver` e a classe `Subject`. A classe `UITween` atua como o observador, responsável por receber e imprimir os resultados da roleta na tela. Ela também notifica a classe `RouletteWheel` por meio de eventos Unity quando o botão de girar é pressionado.

A classe `RouletteWheel` é o sujeito (Subject) e é responsável pela lógica da roleta, incluindo a detecção da cor e da frase correspondente. Ela notifica a classe `UITween` quando a roleta para de girar, fornecendo os resultados.

### Lógica da roleta
A classe `RouletteWheel` implementa a lógica da torreta de girar, dividindo a área em setores para determinar a cor. As cores e as frases são mapeadas através de enumerações para maior legibilidade do código.

## Como Executar o Projeto

1. Clone este repositório.
2. Abra o projeto no Unity.
3. Certifique-se de ter as dependências necessárias, como o TextMeshPro e o LeanTween, instaladas.
4. Execute o jogo no Editor Unity ou compile para a plataforma desejada.
