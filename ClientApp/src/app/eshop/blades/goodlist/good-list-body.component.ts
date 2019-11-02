import { Component, Inject, EventEmitter, Output, Input } from '@angular/core';
import { DataService } from '../../../services/data.service'
import { Good, Category } from '../../../model';
import { createImagePath } from '../../../common'
import { BehaviorSubject } from 'rxjs'

@Component({
    selector: 'good-list-body',
    templateUrl: './good-list-body.component.html',
    styleUrls: ['./../../eshop.component.css']
})

export class GoodListBodyComponent {

    private _goodList: Good[]; // массив товаров

    private _selectedGoodIndex: number;
    private _selectedGoodName: string;

    private _PAGE_SIZE: number = 10;
    private _activePage: number = 1;
    private _goodSize: number;

    private _selectedCategoryId: string;
    private _selectedCategoryName: string;

    public selectedCategoryId(): string {
        return this._selectedCategoryId;
    }
    
    public selectedCategoryName(): string {
        return this._selectedCategoryName;
    }

    private _selectedGoodIds: string[];

    public selectedGoodIds(): string[] {
        return this._selectedGoodIds;
    }

    @Output() goodWasSelected: EventEmitter<Good>;
 
    constructor(@Inject(DataService) private dataService: DataService) {
        this._selectedGoodIndex = -1;
        this._goodList = []
        this._goodSize = 0;
        this._selectedGoodIds = [];
        this.goodWasSelected = new EventEmitter<Good>();
    }

    onNewCategoryWasSelected(category: Category): void {
        this.getGoods(category.id, category.name);
    }

    onPageChange(page: number) {
        this.dataService.getGoods(this._selectedCategoryId, (page - 1) * this._PAGE_SIZE, this._PAGE_SIZE)
            .subscribe((data: Good[]) => {
                this._selectedGoodIndex = -1;
                this._goodList = data;
                this._activePage = page;
            })
    }

    onGoodWasSelected(event: any, index: number): void {
        if (index < this._goodList.length) {
            this._selectedGoodIndex = index;
            this.goodWasSelected.emit(this._goodList[index]);
            this._selectedGoodName = this._goodList[index].name;
        }
    }

    onGoodWasUpdated(updatedGood: Good): void {
        var foundIndex = this._goodList.findIndex(x => x.id == updatedGood.id);
        this._goodList[foundIndex] = updatedGood;
        this._selectedGoodName = this._goodList[this._selectedGoodIndex].name;
    }

    onGoodWasCreated(createdGood: Good): void {
        this.getGoods(this._selectedCategoryId, this._selectedCategoryName);
    }

    onGoodsWasDeleted(deletedGoodIds: string[]): void {
        this.getGoods(this._selectedCategoryId, this._selectedCategoryName);

        deletedGoodIds.forEach(g => {
            const good = this._goodList.find(k => k.id == g);
            var idx = this._goodList.indexOf(good);
            if (idx != -1) {
                --this._goodSize;
                return this._goodList.splice(idx, 1); // The second parameter is the number of elements to remove.
            }
        });
    }

    private getGoods(categoryId: string, categoryName: string): void {
        this.dataService.getGoods(categoryId, (this._activePage - 1) * this._PAGE_SIZE, this._PAGE_SIZE)
            .subscribe((data: Good[]) => {
                this._selectedGoodIndex = -1;
                this._goodList = data;
                this._activePage = 1;
                this._selectedCategoryId = categoryId;
                this._selectedCategoryName = categoryName;
            })

        this.dataService.getGoodsCount(categoryId)
            .subscribe((size: number) => {
                this._goodSize = size;
            });
    }

    createImgPath = (serverPath: string) => {
        return createImagePath(serverPath);
    }

    toogleGoodSelection(evt: any, goodId: string): void {
        if (evt.target.checked) {
            const index = this._selectedGoodIds.indexOf(goodId, 0);
            if (index == -1) {
                this._selectedGoodIds.push(goodId);
            }
        }
        else {
            const index = this._selectedGoodIds.indexOf(goodId, 0);
            if (index > -1) {
                this._selectedGoodIds.splice(index, 1);
            }
        }
    }
}

