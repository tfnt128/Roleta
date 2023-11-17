# Projeto de Roleta Simples

## Descrição
Este projeto implementa uma roleta simples com 4 cores e 3 telas distintas. A interface do usuário (UI) consiste em uma tela inicial com o título "Roleta", a logo da empresa e um botão "Começar". Quando o usuário clica no único botão, a logo e o botão se movem para a esquerda, revelando a roleta com o botão de girar.

Na segunda tela, ao clicar no botão de girar, a roleta gira e uma mensagem do tipo de 4 mensagens pré-programadas é exibida. Quando a roleta para de girar, a mensagem desaparece, e o aplicativo avança para a última tela.

Na terceira tela, a logo da empresa aparece com a cor correspondente à cor que caiu na roleta, juntamente com uma frase abaixo associada à cor. Após isso, uma seta é exibida para reiniciar a fase.

## Lógica do Código

### Padrão de Design Observer
Foi implementado um padrão de design Observer utilizando uma interface `IObserver` e a classe `Subject`. A classe `UITween` atua como o observador, responsável por receber e imprimir os resultados da roleta na tela. Ela também notifica a classe `RouletteWheel` por meio de eventos Unity quando o botão de girar é pressionado.

A classe `RouletteWheel` é o sujeito (Subject) e é responsável pela lógica da roleta, incluindo a detecção da cor e da frase correspondente. Ela notifica a classe `UITween` quando a roleta para de girar, fornecendo os resultados.

### Lógica da Roleta
A classe `RouletteWheel` implementa a lógica da torreta de girar, dividindo a área em setores para determinar a cor. As cores são escaláveis e modulares, permitindo a fácil adição de novas cores, assim como também a adição de novas partições, caso o cliente queira um circulo com mais de 4 faces. Cada cor possui uma enumeração `ColorEnum`, uma cor associada e uma mensagem.

## Como Adicionar Novas Cores
Para adicionar novas cores, siga estes passos simples:

1. Vá ao Inspector do objeto `Roulette object` no Unity.
2. Na seção "Color Mapping", adicione uma nova entrada à lista `colorMappings`.
3. Escolha uma nova enumeração `ColorEnum`, atribua uma cor e uma mensagem correspondente.
4. As novas cores agora podem ser utilizadas em uma roleta futura com mais ou diferentes cores.

## Como Executar o Projeto

1. Clone este repositório.
2. Abra o projeto no Unity.
3. Certifique-se de ter as dependências necessárias, como o TextMeshPro e o LeanTween, instaladas.
4. Execute o jogo no Editor Unity ou compile para a plataforma desejada.
