export function addBearerToken(headers?: HeadersInit): HeadersInit {
    const token = typeof localStorage !== 'undefined' && localStorage.getItem('bearerToken');

    if (!token) {
        return headers;
    }
    return { ...headers, Authorization: `Bearer ${token}` };
}
