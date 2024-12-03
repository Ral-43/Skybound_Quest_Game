# Skybound Quest
Matéria Game Development

Skybound Quest é um jogo de plataforma 2D divertido e desafiador, onde o objetivo é navegar pelas plataformas, superar obstáculos e coletar moedas enquanto você tenta alcançar a casa no topo da tela. Cada nível aumenta o desafio, com inimigos, como espinhos, que causam mais dano conforme o progresso.

---

## 🎮 Como Jogar
- **Movimentação:** Use as teclas **A** e **D** ou as setas direcionais para mover o personagem para a esquerda e para a direita.
- **Pular:** Pressione a tecla **Espaço** para pular.
- **Agachar:** Pressione **S** ou a seta para baixo.
- **Objetivo:** Alcance a casa no topo de cada nível enquanto coleta o máximo de moedas possível.
- **Evite Espinhos:** Os espinhos causam dano ao tocar neles. O dano aumenta conforme o nível.

---

## 🛠️ Recursos
- **Interface Dinâmica:**
  - Contador de moedas coletadas.
  - Indicador de vida.
  - Exibição do nível atual.
- **Progressão de Dificuldade:**
  - Espinhos causam dano prograssivo, dependendo do nível.
- **Sistema de Transição:**
  - Avance para o próximo nível ao alcançar a casa.
- **Tela Final:**
  - Exibe o total de moedas coletadas e mortes após completar o jogo.
- **Menu Principal:**
  - Novo Jogo.
  - Continuar Jogo.
  - Carregar Progresso.
  - Salvar Progresso.
  - Sair do Jogo.

---

## 📂 Estrutura do Jogo
- **Cenas:**
  - `MainMenu`: Tela inicial com opções para iniciar, continuar o jogo e salvar.
  - `Level 1`, `Level 2`, `Level 3`: Fases progressivamente desafiadoras.
  - `End Game`: Tela final que exibe estatísticas do jogador.

- **Scripts Principais:**
  - `GameManager`: Gerencia a lógica global do jogo, como transição de níveis, contagem de moedas e mortes, e salvamento/carregamento de progresso.
  - `Player`: Gerencia os controles e interações do personagem.
  - `LevelTransition`: Cuida da transição entre os níveis.
  - `EndGameManager`: Exibe as estatísticas finais.

---

## 💾 Salvamento e Carregamento
- O progresso é salvo automaticamente ao completar cada nível.
- Use a opção **Salvar Jogo** no menu para armazenar manualmente seu progresso.
- Carregue o jogo salvo com a opção **Carregar Jogo** no menu principal.

---

## 📊 Estatísticas
- **Moedas:** Total de moedas coletadas durante o jogo.
- **Mortes:** Número de mortes acumuladas.

---

## 🎨 Créditos
- **Desenvolvedor:** Renato Andriotti Luiz
- **Curso:** Superior de Tecnologia em Análise e Desenvolvimento de Sistemas - UniFECAP
- **Ferramentas Utilizadas:** Unity, C#, TextMeshPro, Free Platform Game Assets, SunnyLand Free Assets e 2D Casual UI Assets.

---

## 📢 Notas Finais
Skybound Quest foi desenvolvido como parte de um projeto acadêmico para demonstrar habilidades em desenvolvimento de jogos com Unity. É um jogo simples, mas divertido e envolvente, projetado para oferecer uma experiência agradável ao jogador.
