let serverUrl = "http://localhost:10000/";


export const createImagePath = (serverPath: string) => {
    if(serverPath == null) {
        return 'assets/twitter-logo.png';
    }
    else{
        return `${serverUrl}${serverPath}`;
    }
  }
