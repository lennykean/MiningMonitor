export async function handleResponse<T>(response: Response): Promise<T> {
    switch (response.status) {
        case 200:
            return await response.json();
        case 204:
            return;
        case 400:
            throw {
                validation: await response.json(),
            };
        case 401:
            throw {
                unauthorized: true,
            };
        default:
            throw new Error(response.statusText);
    }
}
