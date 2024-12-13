import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 10, // Número de usuários virtuais simultâneos
    duration: '30s', // Duração do teste
};

// Função para testar o endpoint PostMessage (POST)
function testPostMessageEndpoint(baseUrl, queueName) {
    const params = {
        headers: { 'Content-Type': 'application/json' },
    };
    const url = `${baseUrl}/PostMessage?message=Hello&queueName=${queueName}`;
    const res = http.post(url, null, params);
    check(res, {
        'POST /PostMessage status é 200': (r) => r.status === 200,
    });
}

// Função para testar o endpoint PostProductMessage (POST)
function testPostProductMessageEndpoint(baseUrl, queueName) {
    const product = {
        id: 1,
        name: 'Product Name',
        description: 'Product Description',
        publicationDate: new Date().toISOString(),
        category: 'Category Example',
    };
    const params = {
        headers: { 'Content-Type': 'application/json' },
    };
    const url = `${baseUrl}/PostProductMessage?queueName=${queueName}`;
    const res = http.post(url, JSON.stringify(product), params);
    check(res, {
        'POST /PostProductMessage status é 200': (r) => r.status === 200,
    });
}

export default function () {
    const baseUrl = 'https://localhost:7288/api/Base'; // Substitua pela URL da sua API
    const queueNameMessage = 'queue-test';
    const queueNameProduct = 'product-queue';
    
    // testPostMessageEndpoint(baseUrl, queueNameMessage);
    testPostProductMessageEndpoint(baseUrl, queueNameProduct);

    sleep(1); // Pausa de 1 segundo entre as requisições
}
