import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Miner } from '../../models/Miner';
import { MinerService } from '../miner.service';

@Component({
    templateUrl: './miner-edit.component.html'
})
export class MinerEditComponent implements OnInit {
    public miner: Miner;
    public validationErrors: { [key: string]: string[] } = {};

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private minerService: MinerService) {
    }

    public async ngOnInit() {
        this.route.paramMap.subscribe(async paramMap => {
            this.miner = await this.minerService.Get(paramMap.get('id'));
            this.validationErrors = {};
        });
    }

    public async Save(miner: Miner) {
        try {
            this.miner = await this.minerService.Update(miner);
            this.validationErrors = {};
        } catch (error) {
            if (error instanceof HttpErrorResponse && error.status === 400) {
                this.validationErrors = error.error;
            }
        }
    }

    public async Delete() {
        await this.minerService.Delete(this.miner.id);
        this.router.navigateByUrl('/miners');
    }
}
