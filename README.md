# FlexiPay - Stripe Payment Integration with Idempotency Check Using Redis

## Overview

**FlexiPay** is a payment integration solution that facilitates payments using the **Stripe API**. This application ensures idempotency during payment creation to avoid duplicate transactions using **Redis** for storing the idempotency keys. The Redis service is hosted in a Docker container for easy deployment and scalability.

## Features

- **Stripe Payment Integration**: Supports creating and confirming payment intents with Stripe.
- **Idempotency Check**: Ensures that duplicate payment requests are avoided using idempotency keys.
- **Redis for Idempotency Key Management**: Redis is used to store and check the existence of idempotency keys.
- **Containerized Architecture**: Redis is hosted in a Docker container for easy deployment and scalability.

## Prerequisites

Before running the application, ensure that you have the following:

- **Docker**: To run Redis in a container.
- **Stripe Account**: To get your Stripe API keys.
- **.NET SDK**: Ensure you have .NET SDK installed to build and run the application.

  - Install .NET SDK: [Download .NET SDK](https://dotnet.microsoft.com/download)

## Quick Start Guide

### Step 1: Clone the Repository

Clone the repository to your local machine:

```bash
git clone https://github.com/your-repo/flexipay.git
cd flexipay
```

### Step 2: Set Up Redis Using Docker

Start Redis in a Docker Container:

Use the docker-compose.yml file to set up Redis in a container:

```bash
docker-compose up -d
```

This will pull the Redis image and start the Redis container. By default, Redis will be available on port 6379.

Verify Redis is Running:

Check if Redis is running by connecting to the container:

```bash
docker ps
```

If you see the redis container running, Redis is up and ready.

### Step 3: Set Up Stripe API Keys

### Get Your Stripe API Keys:

Log into your Stripe Dashboard.
Under the "Developers" section, you will find your Test Secret Key and Test Publishable Key.

### Configure Your Stripe Keys:

Update appsettings.json file in the root of the project with the following content:

```json
{
  "Stripe": {
    "SecretKey": "your-secret-key"
  }
}
```

Update wwwroot/index.html with PublishableKey:

```js
var stripe = Stripe("enter_your_public_key_here");
```

### Step 4: Build and Run the Application

#### Build the Application:

Run the following command to build the project:

```bash
dotnet build
```

#### Run the Application:

After building the application, run it using:

```bash
dotnet run
```

The application will be available on https://localhost:5001/index.html. APIs can be accessed here https://localhost:5001/swagger/index.html.

### Step 5: Testing the Payment Flow

You can now test the payment flow using the following API endpoints:

#### Create Payment Intent:

Endpoint: POST /api/stripe/create-payment-intent
Request Body:

```json
{
  "amount": 1500,
  "currency": "usd",
  "idempotencyKey": "your-unique-idempotency-key"
}
```

Response: Returns the clientSecret needed to confirm the payment.

#### Confirm Payment Intent:

Endpoint: POST /api/stripe/confirm-payment-intent
Request Body:

```json
{
  "paymentIntentId": "your-payment-intent-id",
  "paymentMethodId": "your-payment-method-id"
}
```

Response: Returns the status of the payment (Payment succeeded or Payment failed).

### Step 6: Storing and Checking Idempotency with Redis

The application uses Redis to store idempotency keys and ensure that each payment request is unique. Here's how the idempotency check works:

#### Check if Idempotency Key Exists:

Before creating a new payment intent, the system checks if the idempotencyKey already exists in Redis.

### Retrieve Existing Payment Intent:

If the key exists, the system retrieves the existing PaymentIntent from Redis and returns the client secret.

#### Create and Store New Payment Intent:

If the key does not exist, the system creates a new payment intent via the Stripe API and stores the result in Redis with a TTL (Time-To-Live) of 24 hours.

### Step 6: Stop the Application and Docker Services:

When you are finished, stop the application by pressing Ctrl + C in the terminal where dotnet run is running.

Also, bring down the Docker containers to stop Redis by running:

```bash
docker-compose down
```
