import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Good } from '../model'

@Injectable({
    providedIn: 'root'
  })
export class DataService {
    constructor(private http: HttpClient) { }

    private goodurl = "/api/good";
    private categoryulr = "/api/category";
    private uploadurl = "api/upload";

    getCategories() {
        return this.http.get(this.categoryulr);
    }

    getGoods(categoryId: string, skip: number, limit: number) {
        const params = new HttpParams().set('skip', skip.toString()).set('limit', limit.toString());
        return this.http.get(this.goodurl + '/' + categoryId, {params});
    }

    getGoodsCount(categoryId: string) {
        return this.http.get(this.goodurl + '/' + categoryId +'/count');
    }

    updateProduct(good: Good) {
        return this.http.put(this.goodurl + '/' + good.id, good);
    }

    createProduct(good: Good) {
        return this.http.post(this.goodurl, good);
    }

    deleteProducts(ids: string[]) {
        let params = new HttpParams();
        ids.forEach((item) => {
            params = params.append(`ids`, item);
          });
        return this.http.delete(this.goodurl, { params });
    }

    uploadFile(files) {
        if (files.length === 0) {
          return;
        }
    
        let fileToUpload = <File>files[0];
        const formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);
    
        return this.http.post(this.uploadurl, formData);
      }

    deleteFile(filePath: string) {
        const params = new HttpParams().set('dbPath', filePath);
        return this.http.delete(this.uploadurl, { params});
    }
    
}
