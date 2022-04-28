import axios from 'axios'

export const BASE_URL = 'https://localhost:7286/';

//in order to avoid typos in components requests
export const ENDPOINTS = {
    participant: 'participant'
}

export const createAPIEndpoint = endpoint => {

    let url = BASE_URL + 'api/' + endpoint + '/';
    return {
        fetch: () => axios.get(url), //to retrieve all of the records
        fetchById: id => axios.get(url + id), //to retrieve one specific record
        post: newRecord => axios.post(url, newRecord),
        put: (id, updateRecord) => axios.put(url + id, updateRecord),
        delete: id => axios.delete(url + id)
        //they will make a request to the corresponding web api methods of the type post get etc
    }
}