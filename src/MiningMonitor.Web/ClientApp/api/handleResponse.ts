export async function handleResponse<T>(response: Response): Promise<T> {
    var err: any;
    switch (response.status) {
        case 200:
            return await response.json();
        case 204:
            return;
        case 400:
            err = new Error(response.statusText);
            err.validation = await response.json();
            throw err;
        case 401:
            err = new Error(response.statusText);
            err.unauthorized = true;
            throw err;
        default:
            throw new Error(response.statusText);
    }
}
