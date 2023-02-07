import axios from 'axios'

async function getDataFromApi() {
  const response = await axios.get('https://localhost:7053/api/v1/WeatherForecast');
  return response.data;
}

export {
  getDataFromApi
}
