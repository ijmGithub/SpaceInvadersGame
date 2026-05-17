# Proyecto Space Invaders

## Arquitectura

El proyecto usa:

- WPF
- MVVM
- C#
- .NET 8

## Reglas

- Separar lógica en ViewModels y Services
- Nunca poner lógica en code-behind
- Usar ICommand con RelayCommand
- Usar ObservableCollection para entidades visuales
- Mantener SOLID
- Código limpio y modular

## Estructura

- Models → entidades del juego
- ViewModels → lógica MVVM
- Views → XAML
- Services → game loop, colisiones y persistencia
- Commands → comandos MVVM

## Juego

El juego es un Space Invaders 2D.

Debe incluir:

- Nave jugador
- Enemigos
- Disparos
- Colisiones
- Sistema de puntuación
- Game loop
- Power ups