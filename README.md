# Thyme_new
## What is it?
It is a neural network which makes use of the modified markov decision process and own conceived time calculus from the thyme_old project to run networked byte code through the neural network

## What does it do?
At present unknown, it calculates and should be able to make predictions based on own logic. The original concept is to make visual predictions on human behaviour by anticipating what will happen in the future within a window of time. However, the logistics of getting all the satellite feeds in at once and making a prediction on human behaviour are astronomical and so what we have here is simply a semi-functioning testbed which needs rebuilding and reimplementing once we know how the new mdp performs as a learning rate according to the time error corrections. 

# Contents

## AIHelpers
thresholds to report 1 or 0 based on true or false in the Y(n) function

Y(n) a boolean logic function which reports to reward, probability and mdp

reward function

probability function

mdp function

## ThymeHelpers
A() a sphere based on radius and dimension (in this case three dimensional)

C() a spherical sigmoid which takes whitenoise and A as an argument to generate a constant for error calculus

SIG() our own sigmoid based on the spherical nature of C()

SIGMOID() standard sigmoid

Cost() a modified cost function

GradientDescent() modified gradient of descent for calculating error rates and learning rates

MSEFullCost() own mean squared error cost function

## Program
Our main program entry point which makes use of a boilerplate tcp socket on the 127.0.0.1 IP for testing purposes and implements the modified boilerplate NeuralNetwork

## thyme
approximation of time velocity v()

Approximation calculus which makes use of our white noise function derived from c() [a spherical sigmoid], predicted output and target output in a three dimensional radius

past time calculus

future time calculus

## NeuralNetwork
A boilerplate Neural network which has been modified to our use case for experimentation in feeds and learning

# References
Mostly derivative of own work and concepts
ChatGPT to speed up the development process and for error checking / boilerplate code
previous references from thyme_old
