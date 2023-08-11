const schools = [
    {
        name: "YorkTown",
        country: "Spain",
    },
    {
        name: "Stanford",
        country: "USA",
    },
    {
        name: "Gymnasium Achern",
        country: "Germany",
    },
];

const editSchoolName = (schools, oldName, newName) =>
    schools.map(({name, ...school}) =>
        (
            {
                ...school,
                name: oldName === name ? newName : name,
            }
        )
    );

const a =
    schools.map((school) => {
        if (school.name === 'YorkTown') {
            return {
                ...school,
                name: 'a',
                country: 'b'
            }
        } else {
            return school;
        }
    });

const updatedSchools = editSchoolName(schools, "YorkTown", "New Gen");
console.log(updatedSchools);
// console.log(schools);
// console.log(a);

const s = {
    name: "YorkTown",
    country: "Spain",
};
const t1 = {...s, name: 'a', country: 'b'};
// console.log(t1);


const t2 = schools.map(sc =>
    ({...s, name: 'a'})
) ;
// console.log(t2);
