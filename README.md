# 2D Movement Controller

This project features a responsive and robust 2D character controller built in Unity, focusing on tight, physics-based controls and a satisfying game feel. It's packed with features found in modern platformers to give players a fluid and intuitive experience.

## Features

### 🎮 Physics-Based Movement

The character's movement is driven by directly manipulating its `Rigidbody2D` velocity. This ensures that movement feels natural and interacts correctly with other physics objects in your world, like moving platforms or pushable boxes.

### 🏃‍♂️ Smooth Ground Control

The character smoothly accelerates to max speed and decelerates to a stop, giving the movement a satisfying sense of weight and momentum.

### 🤸‍♀️ Precise Jumping

Jumping is the core of any platformer, and this controller includes multiple systems to make it feel perfect:

- **Variable Jump Height:** The longer you hold the jump button, the higher the character jumps. This gives players precise control to handle any platforming challenge.
- **Coyote Time:** To improve platforming forgiveness, you can jump for a brief moment _after_ running off a ledge. This small window of opportunity prevents frustrating "I swear I pressed jump!" moments.
- **Jump Buffering:** If you press the jump button slightly _before_ landing, the input is remembered. The character will automatically jump the moment they touch the ground, ensuring no input is ever missed.
