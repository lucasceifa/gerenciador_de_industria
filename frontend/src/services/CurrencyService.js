import axios from 'axios';

const API_URL = 'https://economia.awesomeapi.com.br/json/last/';

const CurrencyService = {
  getRates: (currencies = 'USD-BRL,EUR-BRL') => axios.get(`${API_URL}${currencies}`),
};

export default CurrencyService;